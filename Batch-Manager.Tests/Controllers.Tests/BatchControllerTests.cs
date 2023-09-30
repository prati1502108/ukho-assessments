//using Batch_Manager.Controllers;
//using Batch_Manager.Models;
//using Batch_Manager.Services;
//using Batch_Manager.Tests.Fake;
//using FakeItEasy;
//using Microsoft.AspNetCore.Mvc;
//using NSubstitute;

//namespace Batch_Manager.Tests.Controllers.Tests
//{
//    [TestFixture]
//    public class BatchControllerTests
//    {
//        private IBatch _batch;
//        private CorrelationIdGenerator _correlationIdGenerator;
//        private BatchController _batchController;
//        [OneTimeSetUp]
//        public void Setup()
//        {
//            _batch = Substitute.For<IBatch>();
//            _correlationIdGenerator = Substitute.For<CorrelationIdGenerator>();
//        }

//        [TearDownAttribute]
//        public void TearDown()
//        {
//            _batch = null;
//            _correlationIdGenerator = null;
//            _batchController = null;
//        }

//        [Test]
//        public void When_ValidBatchId_GetBatchByBatchId_ReturnsBatch()
//        {
//            var batchMock = BatchFake.GetFakeBatch();
//            _batchController = new BatchController(_batch, _correlationIdGenerator);
//            _batch.GetBatches("8065e72d-97ca-4d94-b02d-678d6c274fa5").Returns(batchMock);

//            var result = _batchController.GetBatchByBatchId("8065e72d-97ca-4d94-b02d-678d6c274fa5");

//            Assert.IsNotNull(result);
//        }

//        [Test]
//        public void When_InvalidBatchId_GetBatchByBatchId_ReturnsNoRecordFound()
//        {
//            var errorInfo = BatchFake.GetErrorInfo();

//            Batch fakeBatch = new Batch();
//            _batchController = new BatchController(_batch, _correlationIdGenerator);
//            _batch.GetBatches("123").Returns(fakeBatch);

//            var response = _batchController.GetBatchByBatchId("123");
//            var result = response as NotFoundResult;
//            Assert.IsNull(result);
//            Assert.IsNotNull(errorInfo);
//            Assert.That(errorInfo.Errors[0].Description, Is.EqualTo("No Record Found."));
//        }

//        [Test]
//        public void When_BatchIsNotNull_CreateBatch_CreatesBatchInDatabase()
//        {
//            Batch objBatch = new Batch();
//            objBatch = BatchFake.GetFakeBatch();
//            BatchResponse objRes = new BatchResponse();
//            objRes.BatchId = "8065e72d-97ca-4d94-b02d-678d6c274fa5";
//            _batch = Substitute.For<IBatch>();
//            _correlationIdGenerator = Substitute.For<CorrelationIdGenerator>();

//            _batchController = new BatchController(_batch, _correlationIdGenerator);
//            _batch.CreateBatch(objBatch).Returns(objRes);

//            var res = _batchController.CreateBatch(objBatch);
//            Assert.IsNotNull(res);
//        }

//        [Test]
//        public void When_BatchIsNull_CreateBatch_ReturnsErrorDetails()
//        {
//            var errorInfo = BatchFake.GetBatchObjectErrorInfo();
//            Batch objBatch = null;
//            BatchResponse objRes = new BatchResponse();
//            objRes.BatchId = "8065e72d-97ca-4d94-b02d-678d6c274fa5";
//            _batch = Substitute.For<IBatch>();
//            _correlationIdGenerator = Substitute.For<CorrelationIdGenerator>();

//            _batchController = new BatchController(_batch, _correlationIdGenerator);
//            _batch.CreateBatch(objBatch).Returns(objRes);

//            var res = _batchController.CreateBatch(objBatch);
//            Assert.That(errorInfo.Errors[0].Description, Is.EqualTo("Batch object is null."));
//        }
//    }
//}
