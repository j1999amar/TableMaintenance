using AOTableDTOModel;
using AOTableInterface;
using AOTableModel;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Mapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableMaintenance.Controllers;
using Xunit;

namespace TableMaintenanceTest
{
    public class TableMaintenanceControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IAOTableInterface> _tableMock;
        private readonly IMapper _mapper;
        private readonly TableMaintenanceController _sut;

        public TableMaintenanceControllerTest()
        {
            _fixture = new Fixture();
            _tableMock = _fixture.Freeze<Mock<IAOTableInterface>>();
            var mapConfig = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfile>());
            _mapper = mapConfig.CreateMapper();
            _sut = new TableMaintenanceController(_tableMock.Object, _mapper);
        }

        [Fact]
        public async void SearchTable_ShouldReturnOkResponse_WhenAOTableHasTableList()
        {
            //Arrange
            var tableName=_fixture.Create<string>();
            var typeList = _fixture.Create<string[]>();
            var expectedTableList=_fixture.Create<ICollection<AOTable>>();
             _tableMock.Setup(x => x.GetTable(tableName, typeList)).ReturnsAsync(expectedTableList);

            //Act
            var result = await _sut.SearchTable(tableName, typeList);

            //Assertion
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<AOTableDTO>>>();
            result.Result.Should().BeOfType<OkObjectResult>();
            _tableMock.Verify(x=>x.GetTable(tableName, typeList),Times.Once);

        }
        [Fact]
        public async void SearchTable_ShouldReturnNotFoundResponse_WhenAOTableHasZeroData()
        {
            //Arrange
            var tableName = _fixture.Create<string>();
            var typeList = _fixture.Create<string[]>();
            ICollection<AOTable>? expectedTableList = null;
            _tableMock.Setup(x => x.GetTable(tableName, typeList)).ReturnsAsync(expectedTableList);

            //Act
            var result = await _sut.SearchTable(tableName, typeList);

            //Assertion
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<ICollection<AOTableDTO>>>();
            result.Result.Should().BeOfType<NotFoundObjectResult>();
            _tableMock.Verify(x => x.GetTable(tableName, typeList), Times.Once);
        }
        [Fact]
        public async void AddTable_ShouldReturnBadRequestResponse_WhenIdIsAlreadyIsExists()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table=_mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(true);
            _tableMock.Setup(x => x.IsTypeExists(table.Type)).Returns(true);

            //Act
            var result = await _sut.AddTable(tableDTO);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
            _tableMock.Verify(x => x.IsTypeExists(table.Type),Times.Never);

        }
        [Fact]
        public async void AddTable_ShouldReturnOKResponse_WhenIdIsIsNotExists()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(false);
            _tableMock.Setup(x=>x.IsTypeExists(table.Type)).Returns(true);

            //Act
            var result = await _sut.AddTable(tableDTO);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
            _tableMock.Verify(x => x.IsTypeExists(table.Type), Times.Once);
        }
        [Fact]
        public async void AddTable_ShouldReturnBadRequestResponse_WhenTableTypeIsNotExists()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(false);
            _tableMock.Setup(x => x.IsTypeExists(table.Type)).Returns(false);

            //Act
            var result = await _sut.AddTable(tableDTO);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
            _tableMock.Verify(x => x.IsTypeExists(table.Type), Times.Once);
        }
        [Fact]
        public async void AddTable_ShouldReturnOkResponse_WhenTableTypeIsExists()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(false);
            _tableMock.Setup(x => x.IsTypeExists(table.Type)).Returns(true);

            //Act
            var result = await _sut.AddTable(tableDTO);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
            _tableMock.Verify(x => x.IsTypeExists(table.Type), Times.Once);
        }
        [Fact]
        public async void AddTable_ShouldReturnOkResponse_WhenTableDescriptionIsNullOrEmpty()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            tableDTO.Description = null;
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(false);
            _tableMock.Setup(x => x.IsTypeExists(table.Type)).Returns(true);

            //Act
            var result = await _sut.AddTable(tableDTO);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
            _tableMock.Verify(x => x.IsTypeExists(table.Type), Times.Once);
        }
        [Fact]
        public async void ViewTable_ShouldOkresponse_WhenTableIdIsExists()
        {
            //Arrange
            var tableDTO=_fixture.Create<AOTableDTO>();
            var table= _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x=>x.IsExists(table.Id)).Returns(true);

            //Act
            var result = await _sut.ViewTable(tableDTO.Id);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _tableMock.Verify(x=>x.IsExists(table.Id), Times.Once);
        }
        [Fact]
        public async void ViewTable_ShouldOkresponse_WhenTableIdIsNotExists()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(false);

            //Act
            var result = await _sut.ViewTable(tableDTO.Id);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<NotFoundObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
        }








        [Fact]
        public async void EditTable_ShouldReturnBadRequestResponse_WhenIdIsAlreadyIsNotExists()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(false);
            _tableMock.Setup(x => x.IsTypeExists(table.Type)).Returns(false);

            //Act
            var result = await _sut.EditTable(tableDTO);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
            _tableMock.Verify(x => x.IsTypeExists(table.Type), Times.Never);

        }
        [Fact]
        public async void EditTable_ShouldReturnOKResponse_WhenIdIsIsExists()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(true);
            _tableMock.Setup(x => x.IsTypeExists(table.Type)).Returns(true);

            //Act
            var result = await _sut.EditTable(tableDTO);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
            _tableMock.Verify(x => x.IsTypeExists(table.Type), Times.Once);
        }
        [Fact]
        public async void EditTable_ShouldReturnBadRequestResponse_WhenTableTypeIsNotExists()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(true);
            _tableMock.Setup(x => x.IsTypeExists(table.Type)).Returns(false);

            //Act
            var result = await _sut.EditTable(tableDTO);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<BadRequestObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
            _tableMock.Verify(x => x.IsTypeExists(table.Type), Times.Once);
        }
        [Fact]
        public async void EditTable_ShouldReturnOkResponse_WhenTableTypeIsExists()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(true);
            _tableMock.Setup(x => x.IsTypeExists(table.Type)).Returns(true);

            //Act
            var result = await _sut.EditTable(tableDTO);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
            _tableMock.Verify(x => x.IsTypeExists(table.Type), Times.Once);
        }
        [Fact]
        public async void EditTable_ShouldReturnOkResponse_WhenTableDescriptionIsNullOrEmpty()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            tableDTO.Description = null;
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(table.Id)).Returns(true);
            _tableMock.Setup(x => x.IsTypeExists(table.Type)).Returns(true);

            //Act
            var result = await _sut.EditTable(tableDTO);

            //Assertion
            result.Should().NotBeNull();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            _tableMock.Verify(x => x.IsExists(table.Id), Times.Once);
            _tableMock.Verify(x => x.IsTypeExists(table.Type), Times.Once);
        }





        [Fact]
        public void DeleteBrand_ShouldReturnOkResponse_WhenTableIdIsVaildForDelete()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(tableDTO.Id)).Returns(true);
            _tableMock.Setup(x => x.DeleteTable(tableDTO.Id)).Returns(true);

            //Act
            var result = _sut.DeleteTable(tableDTO.Id);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            _tableMock.Verify(x => x.DeleteTable(tableDTO.Id), Times.Once);
            _tableMock.Verify(x => x.IsExists(tableDTO.Id), Times.Once);


        }

        [Fact]
        public void DeleteTable_ShouldReturnNotFoundResponse_WhenTableIdIsNotVaildForDelete()
        {
            //Arrange
            var tableDTO = _fixture.Create<AOTableDTO>();
            var table = _mapper.Map<AOTable>(tableDTO);
            _tableMock.Setup(x => x.IsExists(tableDTO.Id)).Returns(false);
            _tableMock.Setup(x=>x.DeleteTable(tableDTO.Id)).Returns(true);
            //Act
            var result = _sut.DeleteTable(tableDTO.Id);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();
            _tableMock.Verify(x => x.IsExists(tableDTO.Id), Times.Once);
            _tableMock.Verify(x=>x.DeleteTable(tableDTO.Id), Times.Never);
           
        }


    }


}
