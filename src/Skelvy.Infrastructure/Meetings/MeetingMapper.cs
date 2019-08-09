using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Cache;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Messages.Queries;
using Skelvy.Application.Users.Queries;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Infrastructure.Meetings
{
  public class MeetingMapper : IMeetingMapper
  {
    private readonly IMapper _mapper;
    private readonly IMapsService _mapsService;
    private readonly ICacheService _cache;

    public MeetingMapper(IMapper mapper, IMapsService mapsService, ICacheService cache)
    {
      _mapper = mapper;
      _mapsService = mapsService;
      _cache = cache;
    }

    public async Task<SelfModel> Map(User user, Meeting meeting, IList<Message> messages, MeetingRequest meetingRequest, string language)
    {
      var userDto = _mapper.Map<SelfUserDto>(user);
      var meetingModel = await Map(meeting, messages, meetingRequest, language);
      return new SelfModel(userDto, meetingModel);
    }

    public async Task<SelfModel> Map(User user, MeetingRequest meetingRequest, string language)
    {
      var userDto = _mapper.Map<SelfUserDto>(user);
      var meetingModel = await Map(meetingRequest, language);
      return new SelfModel(userDto, meetingModel);
    }

    public SelfModel Map(User user)
    {
      var userDto = _mapper.Map<SelfUserDto>(user);
      return new SelfModel(userDto);
    }

    public async Task<MeetingModel> Map(Meeting meeting, IList<Message> messages, MeetingRequest meetingRequest, string language)
    {
      var meetingDto = _mapper.Map<MeetingDto>(meeting);
      meetingDto.City = await GetCity(meeting.Latitude, meeting.Longitude, language);
      var messagesDto = _mapper.Map<IList<MessageDto>>(messages);
      var requestDto = _mapper.Map<MeetingRequestDto>(meetingRequest);
      requestDto.City = await GetCity(meetingRequest.Latitude, meetingRequest.Longitude, language);
      return new MeetingModel(MeetingRequestStatusType.Found, meetingDto, messagesDto, requestDto);
    }

    public async Task<MeetingModel> Map(MeetingRequest meetingRequest, string language)
    {
      var requestDto = _mapper.Map<MeetingRequestDto>(meetingRequest);
      requestDto.City = await GetCity(meetingRequest.Latitude, meetingRequest.Longitude, language);
      return new MeetingModel(MeetingRequestStatusType.Searching, requestDto);
    }

    public async Task<MeetingSuggestionsModel> Map(IList<MeetingRequest> requests, IList<Meeting> meetings, string language)
    {
      var requestsDto = _mapper.Map<IList<MeetingRequestWithUserDto>>(requests);

      foreach (var request in requestsDto)
      {
        request.City = await GetCity(request.Latitude, request.Longitude, language);
      }

      var meetingsDto = _mapper.Map<IList<MeetingDto>>(meetings);

      foreach (var meeting in meetingsDto)
      {
        meeting.City = await GetCity(meeting.Latitude, meeting.Longitude, language);
      }

      return new MeetingSuggestionsModel(requestsDto, meetingsDto);
    }

    private async Task<string> GetCity(double latitude, double longitude, string language)
    {
      var address = await _cache.GetOrSetData(
        $"maps:reverse#{latitude}#{longitude}#{language}",
        TimeSpan.FromDays(14),
        async () => await _mapsService.Search(latitude, longitude, language));

      return address[0].City;
    }
  }
}
