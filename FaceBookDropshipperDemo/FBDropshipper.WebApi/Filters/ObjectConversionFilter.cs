﻿using System.Collections.Generic;
using System.Dynamic;
using Amazon.Runtime.Internal;
using FBDropshipper.Application.Exceptions;
using FBDropshipper.Domain.Constant;
using FBDropshipper.WebApi.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FBDropshipper.WebApi.Filters
{
    public class TeamMemberMarketplaceCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            var role = context.HttpContext.User.GetRole();
            if (role == RoleNames.TeamMember)
            {
                var marketPlaceIds = user.GetMarketplaces();
                var query = context.HttpContext.Request.Query;
                var key = "MarketPlaceId";
                if (query.ContainsKey(key))
                {
                    var values = query[key];
                    if (values.Any())
                    {
                        var ids = values.Select(int.Parse).ToArray();
                        foreach (int placeId in marketPlaceIds)
                        {
                            if (!ids.Contains(placeId))
                            {
                                throw new BadRequestException("You are not allowed to access this marketplace");
                            }
                        }
                    }
                }
            }
        }
    }
    public class ObjectConversionFilter : ActionFilterAttribute
    {
        private void SetCount(string key, IDictionary<string, object> dynamicObject, HttpRequest request)
        {
            if (dynamicObject.ContainsKey(key))
            {
                var count = (int)dynamicObject[key];
                Dictionary<string, int> pagination = new Dictionary<string, int>();
                pagination.Add("StartIndex",0);
                pagination.Add("Length", count);
                if (request.Query.ContainsKey("page"))
                {
                    var pageString = request.Query["page"].FirstOrDefault();
                    if (int.TryParse(pageString,out int page))
                    {
                        pagination.Add("page",page);
                    }
                }
                else if (request.Query.ContainsKey("Page"))
                {
                    var pageString = request.Query["Page"].FirstOrDefault();
                    if (int.TryParse(pageString,out int page))
                    {
                        pagination.Add("page",page);
                    }
                }
                else
                {
                    pagination.Add("page",1);
                }
                if (request.Query.ContainsKey("pageSize"))
                {
                    var pageString = request.Query["pageSize"].FirstOrDefault();
                    if (int.TryParse(pageString,out int page))
                    {
                        pagination.Add("size",page);
                        if (count % page == 0)
                        {
                            pagination.Add("lastPage", count / page);
                            pagination.Add("endIndex", (count / page) - 1);
                        }
                        else
                        {
                            pagination.Add("lastPage", (count / page) + 1);
                            pagination.Add("endIndex", count / page);
                        }
                    }
                }
                else if (request.Query.ContainsKey("PageSize"))
                {
                    var pageString = request.Query["PageSize"].FirstOrDefault();
                    if (int.TryParse(pageString,out int page))
                    {
                        pagination.Add("size",page);
                        if (count % page == 0)
                        {
                            pagination.Add("lastPage", count / page);
                            pagination.Add("endIndex", (count / page) - 1);
                        }
                        else
                        {
                            pagination.Add("lastPage", (count / page) + 1);
                            pagination.Add("endIndex", count / page);
                        }
                    }
                }
                else
                {
                    int page = 10;
                    pagination.Add("size",page);
                    if (count % page == 0)
                    {
                        pagination.Add("lastPage", count / page);
                        pagination.Add("endIndex", (count / page) - 1);
                    }
                    else
                    {
                        pagination.Add("lastPage", (count / page) + 1);
                        pagination.Add("endIndex", count / page);
                    }
                }
                dynamicObject.Add("Pagination",pagination);
            }
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result as ObjectResult;
            var data = result?.Value;
            if (data != null)
            {
                var dynamicObject = new ExpandoObject() as IDictionary<string, object>;
                foreach (var property in data.GetType().GetProperties()) {
                    dynamicObject.Add(property.Name,property.GetValue(data));
                }

                SetCount("Count", dynamicObject, context.HttpContext.Request);
                
                if (dynamicObject.ContainsKey("success") || dynamicObject.ContainsKey("Success"))
                {
                    return;
                }
                
                dynamicObject.Add("Success",true);
                context.Result = new ContentResult
                {
                    ContentType = "application/json",
                    Content = JsonConvert.SerializeObject(dynamicObject, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }),
                };;
            }
        }
    }
}