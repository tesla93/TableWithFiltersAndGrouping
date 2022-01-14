imageTag="alpine"
if [[ "$1" != "" ]]; then
    echo "Utilizando tag $1 en las imagenes"   
    imageTag="$1" 
fi
dotnet publish FBS_Mantenimientos_Financial.Api/FBS_Mantenimientos_Financial.Api.csproj -c Release  --runtime alpine-x64 --self-contained true
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 119797188488.dkr.ecr.us-east-1.amazonaws.com
TAG=$imageTag docker-compose -f docker-compose-ecs.yml build 
TAG=$imageTag docker-compose -f docker-compose-ecs.yml push