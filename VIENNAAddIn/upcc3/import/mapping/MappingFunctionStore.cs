using System;
using System.Collections.Generic;

namespace VIENNAAddIn.upcc3.import.mapping
{
    internal class MappingFunctionStore
    {
        private readonly Dictionary<string, MappingFunction> mappingFunctions = new Dictionary<string, MappingFunction>();

        public MappingFunctionStore(MapForceMapping mapForceMapping, TargetElementStore targetElementStore)
        {
            foreach (FunctionComponent functionComponent in mapForceMapping.FunctionComponents)
            {
                switch (functionComponent.FunctionType)
                {
                    case "split":
                        {
                            List<object> targetCcElements = new List<object>();

                            foreach (InputOutputKey outputKey in functionComponent.OutputKeys)
                            {
                                string targetElementKey = mapForceMapping.GetMappingTargetKey(outputKey.Value);
                                if (targetElementKey != null)
                                {
                                    targetCcElements.Add(targetElementStore.GetTargetCc(targetElementKey));
                                }
                            }

                            MappingFunction mappingFunction = new MappingFunction(targetCcElements);

                            foreach (InputOutputKey inputKey in functionComponent.InputKeys)
                            {
                                mappingFunctions[inputKey.Value] = mappingFunction;
                            }
                        }
                        break;
                }
            }     
        }

        public MappingFunction GetMappingFunction(string mappingFunctionKey)
        {
            MappingFunction mappingFunction;
            if (mappingFunctions.TryGetValue(mappingFunctionKey, out mappingFunction))
            {
                return mappingFunction;
            }
            return null;
        }
    }
}