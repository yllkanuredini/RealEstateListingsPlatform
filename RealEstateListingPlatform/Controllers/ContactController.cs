using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateListingPlatform.Models; 
using RealEstateListingPlatform.DTOs; 
using RealEstateListingPlatform.Data; 

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContactController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/contact/inquiries
    [HttpPost("inquiries")]
    public async Task<IActionResult> SubmitInquiry([FromBody] InquiryDTO inquiryDto)
    {
        var inquiry = new Inquiry
        {
            PropertyId = inquiryDto.PropertyId,
            UserId = inquiryDto.UserId,
            Message = inquiryDto.Message,
            InquiryDate = DateTime.UtcNow // Assuming we want to set the inquiry date to the current time
        };

        _context.Inquiries.Add(inquiry);
        await _context.SaveChangesAsync();

        var inquiryDetailDto = new InquiryDetailDTO
        {
            Id = inquiry.Id,
            PropertyId = inquiry.PropertyId,
            UserId = inquiry.UserId,
            Message = inquiry.Message,
            InquiryDate = inquiry.InquiryDate
        };

        return CreatedAtAction(nameof(GetInquiryById), new { id = inquiry.Id }, inquiryDetailDto);
    }

    // GET: api/contact/inquiries
    [HttpGet("inquiries")]
    public async Task<IActionResult> GetAllInquiries()
    {
        var inquiries = await _context.Inquiries
            .Select(i => new InquiryDetailDTO
            {
                Id = i.Id,
                PropertyId = i.PropertyId,
                UserId = i.UserId,
                Message = i.Message,
                InquiryDate = i.InquiryDate
            })
            .ToListAsync();

        return Ok(inquiries);
    }

    // GET: api/contact/inquiries/{id}
    [HttpGet("inquiries/{id}")]
    public async Task<IActionResult> GetInquiryById(int id)
    {
        var inquiry = await _context.Inquiries.FindAsync(id);

        if (inquiry == null)
        {
            return NotFound();
        }

        var inquiryDetailDto = new InquiryDetailDTO
        {
            Id = inquiry.Id,
            PropertyId = inquiry.PropertyId,
            UserId = inquiry.UserId,
            Message = inquiry.Message,
            InquiryDate = inquiry.InquiryDate
        };

        return Ok(inquiryDetailDto);
    }

    // DELETE: api/contact/inquiries/{id}
    [HttpDelete("inquiries/{id}")]
    public async Task<IActionResult> DeleteInquiry(int id)
    {
        var inquiry = await _context.Inquiries.FindAsync(id);

        if (inquiry == null)
        {
            return NotFound();
        }

        _context.Inquiries.Remove(inquiry);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/contact/viewings
    [HttpPost("viewings")]
    public async Task<IActionResult> ScheduleViewing([FromBody] ViewingDTO viewingDto)
    {
        var viewing = new Viewing
        {
            PropertyId = viewingDto.PropertyId,
            UserId = viewingDto.UserId,
            ScheduledDate = viewingDto.ScheduledDate,
            Status = "Scheduled" // Assuming a default status of "Scheduled"
        };

        _context.Viewings.Add(viewing);
        await _context.SaveChangesAsync();

        var viewingDetailDto = new ViewingDetailDTO
        {
            Id = viewing.Id,
            PropertyId = viewing.PropertyId,
            UserId = viewing.UserId,
            ScheduledDate = viewing.ScheduledDate,
            Status = viewing.Status
        };

        return CreatedAtAction(nameof(GetViewingById), new { id = viewing.Id }, viewingDetailDto);
    }

    // GET: api/contact/viewings
    [HttpGet("viewings")]
    public async Task<IActionResult> GetAllViewings()
    {
        var viewings = await _context.Viewings
            .Select(v => new ViewingDetailDTO
            {
                Id = v.Id,
                PropertyId = v.PropertyId,
                UserId = v.UserId,
                ScheduledDate = v.ScheduledDate,
                Status = v.Status
            })
            .ToListAsync();

        return Ok(viewings);
    }

    // GET: api/contact/viewings/{id}
    [HttpGet("viewings/{id}")]
    public async Task<IActionResult> GetViewingById(int id)
    {
        var viewing = await _context.Viewings.FindAsync(id);

        if (viewing == null)
        {
            return NotFound();
        }

        var viewingDetailDto = new ViewingDetailDTO
        {
            Id = viewing.Id,
            PropertyId = viewing.PropertyId,
            UserId = viewing.UserId,
            ScheduledDate = viewing.ScheduledDate,
            Status = viewing.Status
        };

        return Ok(viewingDetailDto);
    }

    // PUT: api/contact/viewings/{id}
    [HttpPut("viewings/{id}")]
    public async Task<IActionResult> UpdateViewing(int id, [FromBody] ViewingDTO viewingDto)
    {
        var viewing = await _context.Viewings.FindAsync(id);

        if (viewing == null)
        {
            return NotFound();
        }

        viewing.PropertyId = viewingDto.PropertyId;
        viewing.UserId = viewingDto.UserId;
        viewing.ScheduledDate = viewingDto.ScheduledDate;
        // Status is not updated here, assuming it's managed elsewhere

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/contact/viewings/{id}
    [HttpDelete("viewings/{id}")]
    public async Task<IActionResult> CancelViewing(int id)
    {
        var viewing = await _context.Viewings.FindAsync(id);

        if (viewing == null)
        {
            return NotFound();
        }

        _context.Viewings.Remove(viewing);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}