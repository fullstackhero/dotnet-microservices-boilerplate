BUILD_CONFIGURATION := Release

gateway:
	echo "running dotnet publish.."
	cd ./projects/fluentpos-lite/src/api-gateway/Gateway && dotnet publish --os linux --arch x64 -c $(BUILD_CONFIGURATION) /p:PublishProfile=DefaultContainer /p:ContainerImageTags=latest --self-contained

identity:
	echo "running dotnet publish.."
	cd ./projects/fluentpos-lite/src/identity-server/Host && dotnet publish --os linux --arch x64 -c $(BUILD_CONFIGURATION) /p:PublishProfile=DefaultContainer /p:ContainerImageTags=latest --self-contained

catalog:
	echo "running dotnet publish.."
	cd ./projects/fluentpos-lite/src/services/catalog/Host && dotnet publish --os linux --arch x64 -c $(BUILD_CONFIGURATION) /p:PublishProfile=DefaultContainer /p:ContainerImageTags=latest --self-contained

infra:
	cd ./projects/fluentpos-lite/deployments/docker-compose && docker-compose -f infrastructure.yml up

services:
	cd ./projects/fluentpos-lite/deployments/docker-compose && docker-compose -f docker-compose.yml up

all:
	cd ./projects/fluentpos-lite/deployments/docker-compose && docker-compose -f infrastructure.yml -f docker-compose.yml up
