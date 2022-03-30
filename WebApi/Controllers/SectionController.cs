using Business.Abstract;
using DataAccess.Entities.Section;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionService _sectionService;

        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        [HttpPost("CreateSection")]
        public async Task<IActionResult> CreateSection(Section section)
        {
            var result = await _sectionService.Create(section);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpGet("GetAllSection")]
        public IActionResult GetAllSection()
        {
            var result = _sectionService.GetAll();

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

       

        [HttpGet("GetSectionById/{sectionId}")]
        public IActionResult GetSectionById(int sectionId)
        {
            var result = _sectionService.GetById(sectionId);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpDelete("DeleteSection/{sectionId}")]
        public async Task<IActionResult> DeleteSection(int sectionId)
        {
            var result = await _sectionService.Delete(sectionId);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }

        [HttpPut("UpdateSection")]
        public async Task<IActionResult> UpdateSection(int sectionId, Section section)
        {


            var result = await _sectionService.Update(sectionId, section);

            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }
    }
}
