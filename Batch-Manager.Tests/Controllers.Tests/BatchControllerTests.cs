using Batch_Manager.Controllers;
using Batch_Manager.Models;
using Batch_Manager.Services;
using Batch_Manager.Tests.Fake;
using Batch_Manager.Validators;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Batch_Manager.Tests.Controllers.Tests
{
    [TestFixture]
    public class BatchControllerTests
    {
        private IBatch _batch;
        private IBatchValidator _batchValidator;
        private CorrelationIdGenerator _correlationIdGenerator;
        private BatchController _batchController;
        [OneTimeSetUp]
        public void Setup()
        {
            _batch = Substitute.For<IBatch>();
            _batchValidator = Substitute.For<IBatchValidator>();
            _correlationIdGenerator = Substitute.For<CorrelationIdGenerator>();
            _batchController = new BatchController(_batch, _correlationIdGenerator, _batchValidator);
        }

        [TearDownAttribute]
        public void TearDown()
        {
            _batch = null;
            _correlationIdGenerator = null;
            _batchController = null;
            _batchValidator = null;
        }

        [Test]
        public void When_ValidBatchId_GetBatchByBatchId_ReturnsBatch()
        {
            var batchMock = BatchFake.GetFakeBatch();
            _batch.GetBatches("8065e72d-97ca-4d94-b02d-678d6c274fa5").Returns(batchMock);
            var result = _batchController.GetBatchByBatchId("8065e72d-97ca-4d94-b02d-678d6c274fa5");
            Assert.IsNotNull(result);
        }

        [Test]
        public void When_InvalidBatchId_GetBatchByBatchId_ReturnsNoRecordFound()
        {
            var errorDetails = BatchFake.GetBatchRecordsErrorDetails();

            Batch fakeBatch = new Batch();
            _batch.GetBatches("123").Returns(fakeBatch);

            var response = _batchController.GetBatchByBatchId("123");
            var result = response as NotFoundResult;
            Assert.IsNull(result);
            Assert.IsNotNull(errorDetails);
            Assert.That(errorDetails.Errors[0].Description, Is.EqualTo("No Record Found."));
        }

        [Test]
        public void When_BatchIsNotNull_CreateBatch_CreatesBatchRecord()
        {
            Batch batch = new Batch();
            batch = BatchFake.GetFakeBatch();
            BatchResponse batchResponse = new BatchResponse();
            batchResponse.BatchId = "8065e72d-97ca-4d94-b02d-678d6c274fa5";
            _batch = Substitute.For<IBatch>();
            _correlationIdGenerator = Substitute.For<CorrelationIdGenerator>();
            _batchController = new BatchController(_batch, _correlationIdGenerator, _batchValidator);
            _batch.CreateBatch(batch).Returns(batchResponse);

            var result = _batchController.CreateBatch(batch);
            Assert.IsNotNull(result);
        }

        [Test]
        public void When_BatchIsNull_CreateBatch_ReturnsErrorDetails()
        {
            var errorDetails = BatchFake.GetBatchObjectErrorDetails();
            Batch batch = null;
            BatchResponse batchResponse = new BatchResponse();
            batchResponse.BatchId = "8065e72d-97ca-4d94-b02d-678d6c274fa5";
            _batch = Substitute.For<IBatch>();
            _correlationIdGenerator = Substitute.For<CorrelationIdGenerator>();

            _batchController = new BatchController(_batch, _correlationIdGenerator, _batchValidator);
            _batch.CreateBatch(batch).Returns(batchResponse);

            var res = _batchController.CreateBatch(batch);
            Assert.That(errorDetails.Errors[0].Description, Is.EqualTo("Batch object is null."));
        }
    }
}
