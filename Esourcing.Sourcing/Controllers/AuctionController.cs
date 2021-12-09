using Esourcing.Sourcing.Data.Abstract;
using Esourcing.Sourcing.Entities;
using Esourcing.Sourcing.Repositories.Abstract;
using Esourcing.Sourcing.Repositories.Concrete;
using EventBusRabbitMQ.Events.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Esourcing.Sourcing.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly ILogger<AuctionController> _logger;
        private readonly IBidRepository _bidRepository;
        public AuctionController(IAuctionRepository auctionRepository,IBidRepository bidRepository, ILogger<AuctionController> logger)
        {
            _auctionRepository = auctionRepository;
            _logger = logger;
            _bidRepository = bidRepository;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Auction>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Auction>>> GetAuctions()
        {
            var auctions = await _auctionRepository.GetAuctions();
            return Ok(auctions);
        }
        [HttpGet("{id:length(24)}", Name = "GetAuction")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Auction>> GetAuction(string id)
        {
            var auction = await _auctionRepository.GetAuction(id);
            if (auction == null)
            {
                _logger.LogError($"Auction with id : {id} hasn't been found in the database.");
                return NotFound();
            }
            return Ok(auction);
        }
        [HttpPost]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Auction>> CreateAuction([FromBody] Auction auction)
        {
            await _auctionRepository.Create(auction);
            return CreatedAtRoute("GetAuction", new { id = auction.Id });
        }

        [HttpPut]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> UpdateAuction([FromBody] Auction auction)
        {
            return Ok(await _auctionRepository.Update(auction));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Auction), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Auction>> DeleteAuctionById(string id)
        {
            return Ok(await _auctionRepository.Delete(id));
        }

        [HttpPost("CompleteAuction")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CompleteAuction(string id)
        {
            Auction auction = await _auctionRepository.GetAuction(id);
            if (auction ==null)
            {
                return NotFound();
            }
            if (auction.Status!=(int)Status.Active)
            {
                _logger.LogError("Auction can not be completed.");
                return BadRequest();
            }

            Bid bid = await _bidRepository.GetWinnerBid(id);
            if (bid==null)
            {
                return NotFound();
            }
            
            
            


        }
    }
}
