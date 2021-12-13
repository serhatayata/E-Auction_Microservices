﻿using ESourcing.Core.Common;
using ESourcing.Core.ResultModels;
using ESourcing.UI.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ESourcing.UI.Clients
{
    public class BidClient
    {
        public HttpClient _client { get; }

        public BidClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(CommonInfo.LocalAuctionBaseAddress);
        }

        public async Task<Result<List<BidViewModel>>> GetAllBidsByAuctionId(string id)
        {
            var response = await _client.GetAsync("/api/v1/Bid/GetAllBidsByAuctionId?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<BidViewModel>>(responseData);
                if (result.Any())
                {
                    return new Result<List<BidViewModel>>(true, ResultConstant.RecordFound, result.ToList());
                }
                else
                {
                    return new Result<List<BidViewModel>>(false, ResultConstant.NotFound);
                }
            }
            return new Result<List<BidViewModel>>(false, ResultConstant.NotFound);
        }






    }
}
