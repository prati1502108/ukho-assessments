﻿namespace Batch_Manager.Services
{
    public class CorrelationIdGenerator : ICorrelationIdGenerator
    {
        private string _correlationId = Guid.NewGuid().ToString();
        public string Get()
        {
            return _correlationId;
        }

        public void Set(string correlationId)
        {
            _correlationId = correlationId;
        }
    }
}
