using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Skelvy.Application.Core.Cache;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Domain.Entities;

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

    public async Task<MeetingDto> Map(Meeting meeting, string language)
    {
      var meetingDto = _mapper.Map<MeetingDto>(meeting);
      meetingDto.City = await GetCity(meeting.Latitude, meeting.Longitude, language);
      return meetingDto;
    }

    public async Task<IList<MeetingDto>> Map(IList<Meeting> meetings, string language)
    {
      var meetingsDto = new List<MeetingDto>();

      foreach (var meeting in meetings)
      {
        var meetingDto = await Map(meeting, language);
        meetingsDto.Add(meetingDto);
      }

      return meetingsDto;
    }

    public async Task<MeetingRequestDto> Map(MeetingRequest meetingRequest, string language)
    {
      var requestDto = _mapper.Map<MeetingRequestDto>(meetingRequest);
      requestDto.City = await GetCity(meetingRequest.Latitude, meetingRequest.Longitude, language);
      return requestDto;
    }

    public async Task<IList<MeetingRequestDto>> Map(IList<MeetingRequest> meetingRequests, string language)
    {
      var requestsDto = new List<MeetingRequestDto>();

      foreach (var request in meetingRequests)
      {
        var requestDto = await Map(request, language);
        requestsDto.Add(requestDto);
      }

      return requestsDto;
    }

    public async Task<MeetingInvitationDto> Map(MeetingInvitation meetingInvitation, string language)
    {
      var meetingInvitationDto = _mapper.Map<MeetingInvitationDto>(meetingInvitation);
      meetingInvitationDto.Meeting.City = await GetCity(meetingInvitation.Meeting.Latitude, meetingInvitation.Meeting.Longitude, language);
      return meetingInvitationDto;
    }

    public async Task<IList<MeetingInvitationDto>> Map(IList<MeetingInvitation> meetingInvitations, string language)
    {
      var meetingInvitationsDto = new List<MeetingInvitationDto>();

      foreach (var meetingInvitation in meetingInvitations)
      {
        var meetingInvitationDto = await Map(meetingInvitation, language);
        meetingInvitationsDto.Add(meetingInvitationDto);
      }

      return meetingInvitationsDto;
    }

    public async Task<MeetingSuggestionsModel> Map(IList<MeetingRequest> requests, IList<Meeting> meetings, string language)
    {
      var requestsDto = _mapper.Map<IList<MeetingRequestWithUserDto>>(requests);

      foreach (var request in requestsDto)
      {
        request.City = await GetCity(request.Latitude, request.Longitude, language);
      }

      var meetingsDto = _mapper.Map<IList<MeetingWithUsersDto>>(meetings);

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
