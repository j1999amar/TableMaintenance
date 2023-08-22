using AOTableDTOModel;
using AOTableInterface;
using AutoMapper;
using Mapper;
using Microsoft.AspNetCore.Mvc;

namespace TableMaintenance.Controllers
{
    [ApiController]
    public class TableMaintenanceController : Controller
    {
        private readonly IAOTableInterface _tableInterface;
        private readonly IMapper _mapper;

        public TableMaintenanceController(IAOTableInterface tableInterface, IMapper mapper)
        {
            _tableInterface = tableInterface;
            _mapper = mapper;
        }
        #region Search Table
        [HttpPost]
        [Route("[controller]/SearchTable")]
        public async Task<ActionResult<ICollection<AOTableDTO>>> SearchTable([FromQuery] string? tableName, [FromQuery] string[]? typeList) 
        {
            try
            {
                var tableList = await _tableInterface.GetTable(tableName, typeList);
                var tableListDto = _mapper.Map<ICollection<AOTableDTO>>(tableList);
                if(tableListDto.Count==0 )
                {
                    return NotFound("Data Not Found");
                }
                else
                {
                    return Ok(tableListDto);
                }
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
