using Esourcing.Sourcing.Data.Abstract;
using Esourcing.Sourcing.Entities;
using Esourcing.Sourcing.Repositories.Abstract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esourcing.Sourcing.Repositories.Concrete
{
    public class BidRepository : IBidRepository
    {
        private readonly ISourcingContext _context;
        public BidRepository(ISourcingContext context)
        {
            _context = context;
        }
        public async Task<List<Bid>> GetBidsByAuctionId(string id)
        {
            FilterDefinition<Bid> filter = Builders<Bid>.Filter.Eq(x => x.AuctionId, id);
            List<Bid> bids = await _context.Bids.Find(filter).ToListAsync();
            bids = bids.OrderByDescending(x => x.CreatedAt)
                        .GroupBy(x => x.SellerUserName)
                        .Select(a => new Bid
                        {
                            AuctionId = a.FirstOrDefault().AuctionId,
                            Id = a.FirstOrDefault().Id,
                            ProductId = a.FirstOrDefault().ProductId,
                            Price = a.FirstOrDefault().Price,
                            SellerUserName = a.FirstOrDefault().SellerUserName,
                            CreatedAt = a.FirstOrDefault().CreatedAt
                        }).ToList();
            return bids;
        }

        public async Task<Bid> GetWinnerBid(string id)
        {
            List<Bid> bids = await GetBidsByAuctionId(id);
            return bids.OrderByDescending(x=>x.Price).FirstOrDefault();
        }

        public async Task SendBid(Bid bid)
        {
            await _context.Bids.InsertOneAsync(bid);
        }
    }
}
