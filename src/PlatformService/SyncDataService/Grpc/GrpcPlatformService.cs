

using AutoMapper;
using Grpc.Core;
using PlatformService.Data;
using PlatformService.Grpc;

namespace PlatformService.SyncDataService.Grpc
{   
    public  class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;

        public GrpcPlatformService(IPlatformRepo platformRepo,IMapper mapper)
        {
            _platformRepo= platformRepo;
            _mapper = mapper;
        }

        public override Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext serverCallContext)
        {
            var response = new PlatformResponse();
            var platformItems= _platformRepo.GetAllPlatform();
            foreach (var platform in platformItems)
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
                
            }
            return Task.FromResult(response);
        }



    }
    
}