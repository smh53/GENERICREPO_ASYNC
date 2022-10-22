using Business.Abstract;
using Business.Validation.FluentValidation;
using DataAccess.Entities.Section;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateSection")]
        public async Task<IActionResult> CreateSection(Section section)
        {
            var result = await _sectionService.Create(section);
           
                return Ok(result);     
            
        }
      
        [HttpGet("GetAllSections")]
        public IActionResult GetAllSections()
        {
            var result = _sectionService.GetAll();     
                return Ok(result);              
        }

       

        [HttpGet("GetSectionById/{sectionId}")]
        public IActionResult GetSectionById(int sectionId)
        {
            var result = _sectionService.GetById(sectionId);

           
                return Ok(result);            
          
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteSection/{sectionId}")]
        public async Task<IActionResult> DeleteSection(int sectionId)
        {
            var result = await _sectionService.Delete(sectionId);
           
                return Ok(result);            
          
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateSection")]
        public async Task<IActionResult> UpdateSection(int sectionId, Section section)
        {       
            var result = await _sectionService.Update(sectionId, section);
         
                return Ok(result);                    
        }
    }
}
