using System;
using Microsoft.Extensions.Logging;
using Shoko.Server.Providers.AniDB.Interfaces;
using Shoko.Server.Providers.AniDB.UDP.Exceptions;
using Shoko.Server.Providers.AniDB.UDP.Generic;

namespace Shoko.Server.Providers.AniDB.UDP.User;

/// <summary>
/// Add a file to MyList. If it doesn't exist, it will return the MyListID for future updates.
/// If it exists, it will return the current status on AniDB. 
/// </summary>
public class RequestAddFile : UDPRequest<ResponseMyListFile>
{
    // These are dependent on context
    protected override string BaseCommand
    {
        get
        {
            var command = $"MYLISTADD size={Size}&ed2k={Hash}&state={(int)State}";
            if (IsWatched)
            {
                var date = WatchedDate ?? DateTime.Now;
                command += $"&viewed=1&viewdate={AniDBExtensions.GetAniDBDateAsSeconds(date)}";
            }
            else
            {
                command += "&viewed=0";
            }

            return command;
        }
    }

    public string Hash { get; set; }

    public long Size { get; set; }

    public MyList_State State { get; set; }

    public bool IsWatched { get; set; }
    public DateTime? WatchedDate { get; set; }

    protected override UDPResponse<ResponseMyListFile> ParseResponse(UDPResponse<string> response)
    {
        var code = response.Code;
        var receivedData = response.Response;
        switch (code)
        {
            case UDPReturnCode.MYLIST_ENTRY_ADDED:
                {
                    /* Response Format
                     * {int4 mylist id of new entry}
                     */
                    // parse the MyList ID
                    if (!int.TryParse(receivedData, out var myListID))
                    {
                        throw new UnexpectedUDPResponseException(code, receivedData, Command);
                    }

                    return new UDPResponse<ResponseMyListFile>
                    {
                        Code = code,
                        Response = new ResponseMyListFile
                        {
                            MyListID = myListID,
                            State = State,
                            IsWatched = IsWatched,
                            WatchedDate = WatchedDate,
                            UpdatedAt = DateTime.Now
                        }
                    };
                }
            case UDPReturnCode.FILE_ALREADY_IN_MYLIST:
                {
                    /* Response Format
                     * {int4 lid}|{int4 fid}|{int4 eid}|{int4 aid}|{int4 gid}|{int4 date}|{int2 state}|{int4 viewdate}|{str storage}|{str source}|{str other}|{int2 filestate}
                     */
                    //file already exists: read 'watched' status
                    var arrStatus = receivedData.Split('|');
                    if (!int.TryParse(arrStatus[0], out var myListID))
                    {
                        throw new UnexpectedUDPResponseException("MyListID was not provided. Use AniDBMyList_RequestAddEpisode for generic files.",
                            response: receivedData, code: code, request: Command);
                    }

                    var state = (MyList_State)int.Parse(arrStatus[6]);

                    var viewdate = int.Parse(arrStatus[7]);
                    var updatedate = int.Parse(arrStatus[5]);
                    var watched = viewdate > 0;
                    DateTime? updatedAt = null;
                    DateTime? watchedDate = null;
                    if (updatedate > 0)
                    {
                        updatedAt = DateTime.UnixEpoch
                            .AddSeconds(updatedate)
                            .ToLocalTime();
                    }

                    if (watched)
                    {
                        watchedDate = DateTime.UnixEpoch
                            .AddSeconds(viewdate)
                            .ToLocalTime();
                    }

                    return new UDPResponse<ResponseMyListFile>
                    {
                        Code = code,
                        Response = new ResponseMyListFile
                        {
                            MyListID = myListID,
                            State = state,
                            IsWatched = watched,
                            WatchedDate = watchedDate,
                            UpdatedAt = updatedAt
                        }
                    };
                }
            case UDPReturnCode.NO_SUCH_FILE:
            case UDPReturnCode.NO_SUCH_EPISODE:
                return new UDPResponse<ResponseMyListFile> { Code = code };
        }

        throw new UnexpectedUDPResponseException(code, receivedData, Command);
    }

    public RequestAddFile(ILoggerFactory loggerFactory, IUDPConnectionHandler handler) : base(loggerFactory, handler)
    {
    }
}
