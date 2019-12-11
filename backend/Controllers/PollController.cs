using BabyPoll.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BabyPoll.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class PollController : ControllerBase
    {
        private readonly BabyPollContext context;

        public PollController(BabyPollContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePoll([FromBody] Poll poll)
        {
            poll.PollId = Guid.NewGuid();
            poll.IsOpen = true;
            context.Polls.Add(poll);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<List<Poll>> GetPolls(int? skip, int? take)
        {
            var polls = context.Polls.AsQueryable();
            if (skip.HasValue)
            {
                polls = polls.Skip(skip.Value);
            }
            if (take.HasValue)
            {
                polls = polls.Take(take.Value);
            }
            return await polls
                .Where(p => p.IsOpen)
                .Include(p => p.Entries)
                .ToListAsync();
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Poll>> GetPoll(Guid id)
        {
            var poll = await GetPollById(id);
            if (poll != null)
            {
                return poll;
            }
            return NotFound();
        }

        private async Task<Poll> GetPollById(Guid id)
        {
            return await context.Polls
               .Where(p => p.PollId == id)
               .Include(p => p.Entries)
               .SingleOrDefaultAsync();
        }

        [HttpPut()]
        [Authorize("update:poll")]
        public async Task<IActionResult> UpdatePoll(Guid id, [FromBody] Poll poll)
        {
            var currentPoll = await GetPollById(id);
            if (currentPoll != null)
            {
                currentPoll.Name = poll.Name;
                currentPoll.Url = poll.Url;
                currentPoll.IsOpen = poll.IsOpen;
                await context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return await CreatePoll(poll);
            }
        }

        [HttpPut("{id:Guid}/entry")]
        public async Task<IActionResult> AddEntry(Guid id, [FromBody] Entry entry)
        {
            var poll = await GetPollById(id);
            if (poll != null)
            {
                if (poll.Entries.Any(e => e.Guess == entry.Guess || e.Participant == entry.Participant))
                {
                    return Ok();
                }
                entry.PollId = poll.PollId;
                entry.EntryId = Guid.NewGuid();
                context.Entries.Add(entry);
                poll.Entries.Add(entry);
                await context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpDelete()]
        [Authorize("delete:poll")]
        public async Task<IActionResult> DeletePoll(Guid id)
        {
            var poll = await context.Polls.FindAsync(id);
            if (poll != null)
            {
                context.Polls.Remove(poll);
                await context.SaveChangesAsync();
            }
            return Ok();
        }
    }
}