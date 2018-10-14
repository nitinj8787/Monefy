using AutoMapper;

namespace MonefyApi.Mapper
{
    public interface IAutoMapperTypeConfigurator
    {
        void Configure(IMapperConfigurationExpression configuration);
    }
}