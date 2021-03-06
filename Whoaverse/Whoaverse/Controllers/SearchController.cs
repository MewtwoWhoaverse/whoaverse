﻿/*
This source file is subject to version 3 of the GPL license, 
that is bundled with this package in the file LICENSE, and is 
available online at http://www.gnu.org/licenses/gpl.txt; 
you may not use this file except in compliance with the License. 

Software distributed under the License is distributed on an "AS IS" basis,
WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
the specific language governing rights and limitations under the License.

All portions of the code written by Whoaverse are Copyright (c) 2014 Whoaverse
All Rights Reserved.
*/

using PagedList;
using System.Linq;
using System.Web.Mvc;
using Whoaverse.Models;
using Whoaverse.Utils;

namespace Whoaverse.Controllers
{
    public class SearchController : Controller
    {
        private whoaverseEntities db = new whoaverseEntities();

        [PreventSpam]
        public ActionResult SearchResults(int? page, string q, string l, string sub)
        {
            if (q != null && q.Length >= 3)
            {
                //limit the search to selected subverse
                if (l != null && sub != null)
                {
                    //ViewBag.SelectedSubverse = string.Empty;
                    ViewBag.SearchTerm = q;                    

                    int pageSize = 25;
                    int pageNumber = (page ?? 1);

                    //show search results, default sorting by rank and date
                    var linkSubmissions = db.Messages
                        .Where(x => x.Name != "deleted" & x.Subverse == sub & x.Linkdescription.ToLower().Contains(q))
                        .OrderByDescending(s => s.Rank)
                        .ThenByDescending(s => s.Date).Take(100).ToList();

                    var selfSubmissionsTitle = db.Messages
                        .Where(x => x.Name != "deleted" & x.Subverse == sub & x.Title.ToLower().Contains(q))
                        .OrderByDescending(s => s.Rank)
                        .ThenByDescending(s => s.Date).Take(100).ToList();

                    var selfSubmissionsMessageContent = db.Messages
                        .Where(x => x.Name != "deleted" & x.Subverse == sub & x.MessageContent.ToLower().Contains(q))
                        .OrderByDescending(s => s.Rank)
                        .ThenByDescending(s => s.Date).Take(100).ToList();

                    var results = linkSubmissions.Concat(selfSubmissionsTitle);
                    results = results.Concat(selfSubmissionsMessageContent);

                    ViewBag.Title = "search results";

                    return View("~/Views/Search/Index.cshtml", results.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    ViewBag.SelectedSubverse = string.Empty;
                    ViewBag.SearchTerm = q;

                    int pageSize = 25;
                    int pageNumber = (page ?? 1);

                    //show search results, default sorting by rank and date
                    var linkSubmissions = db.Messages
                        .Where(x => x.Name != "deleted" & x.Linkdescription.ToLower().Contains(q))
                        .OrderByDescending(s => s.Rank)
                        .ThenByDescending(s => s.Date).Take(100).ToList();

                    var selfSubmissionsTitle = db.Messages
                        .Where(x => x.Name != "deleted" & x.Title.ToLower().Contains(q))
                        .OrderByDescending(s => s.Rank)
                        .ThenByDescending(s => s.Date).Take(100).ToList();

                    var selfSubmissionsMessageContent = db.Messages
                        .Where(x => x.Name != "deleted" & x.MessageContent.ToLower().Contains(q))
                        .OrderByDescending(s => s.Rank)
                        .ThenByDescending(s => s.Date).Take(100).ToList();

                    var results = linkSubmissions.Concat(selfSubmissionsTitle);
                    results = results.Concat(selfSubmissionsMessageContent);

                    ViewBag.Title = "search results";

                    return View("~/Views/Search/Index.cshtml", results.ToPagedList(pageNumber, pageSize));
                }
                
            }
            else
            {
                return new EmptyResult();
            }

        }
    }
}