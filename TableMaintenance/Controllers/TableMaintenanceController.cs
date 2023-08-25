using AOTableDTOModel;
using AOTableInterface;
using AOTableModel;
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
        [HttpGet]
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
        #region Add Table
        [HttpPost]
        [Route("[controller]/AddTable")]
        public async Task<ActionResult<AOTableDTO>> AddTable([FromBody]AOTableDTO tableDTO)
        {
            try
            {
                if (_tableInterface.IsExists(tableDTO.Id))
                {
                    return BadRequest("Id is Already is in database ");
                }
                if (!_tableInterface.IsTypeExists(tableDTO.Type))
                {
                    return BadRequest("Type is not exsits");
                }
                else
                {
                    if (string.IsNullOrEmpty(tableDTO.Description))
                    {
                        tableDTO.Description = tableDTO.Name;
                    }
                    var table=_mapper.Map<AOTable>(tableDTO);
                    await _tableInterface.AddTable(table);
                    return Ok(table);
                }
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region View Table
        [HttpGet]
        [Route("[controller]/ViewTable/{id}")]
        public async Task<ActionResult<AOTableDTO>> ViewTable([FromRoute]Guid id)
        {
            try
            {
                if (_tableInterface.IsExists(id))
                {
                    var table = await _tableInterface.ViewTable(id);
                    return Ok(table);
                }
                else
                {
                    return NotFound("Table not found");
                }
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        #region Edit Table
        [HttpPut]
        [Route("[controller]/EditTable")]
        public async Task<ActionResult<AOTableDTO>> EditTable([FromBody] AOTableDTO tableDTO)
        {
            try
            {
                if (_tableInterface.IsExists(tableDTO.Id))
                {
                    if (!_tableInterface.IsTypeExists(tableDTO.Type))
                    {
                        return BadRequest("Type is not exsits");
                    }
                    if (string.IsNullOrEmpty(tableDTO.Description))
                    {
                        tableDTO.Description = tableDTO.Name;
                    }
                    var table = _mapper.Map<AOTable>(tableDTO);
                    await _tableInterface.EditTable(table);
                    return Ok(table);
                }
                else
                {
                    return BadRequest("Id Not Found");
                }    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        # region Delete Table
        [HttpDelete]
        [Route("[controller]/deleteTable/{id}")]
        public ActionResult DeleteTable([FromRoute] Guid id)
        {
            try
            {
                if ( _tableInterface.IsExists(id))
                {
                    _tableInterface.DeleteTable(id);
                    var data= Json(new { status = "Deleted" });

                    return Ok(data);
                }
                return NotFound("Id Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
