using System;
using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Queries.FindMessages
{
  public class FindMessagesQuery : IQuery<IList<MessageDto>>
  {
    public FindMessagesQuery(int userId, DateTimeOffset beforeDate)
    {
      UserId = userId;
      BeforeDate = beforeDate;
    }

    public FindMessagesQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public DateTimeOffset BeforeDate { get; set; }
  }
}
