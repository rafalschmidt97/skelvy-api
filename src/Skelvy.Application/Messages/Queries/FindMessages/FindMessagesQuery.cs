using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Messages.Queries.FindMessages
{
  public class FindMessagesQuery : IQuery<IList<MessageDto>>
  {
    public FindMessagesQuery(int groupId, int userId, DateTimeOffset beforeDate)
    {
      GroupId = groupId;
      UserId = userId;
      BeforeDate = beforeDate;
    }

    [JsonConstructor]
    public FindMessagesQuery()
    {
    }

    public int GroupId { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset BeforeDate { get; set; }
  }
}
