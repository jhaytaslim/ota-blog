# load env variables
[ ! -f .env ] || export $(grep -v '^#' .env | xargs)

#function to iterate migration 
nowInMs() {
  echo "$(($(date +'%s * 1000 + %-N / 1000000')))"
}

# set the dotnet installer
dotnet new tool-manifest
dotnet tool install dotnet-ef

#run and apply migration
cd src/API
dotnet ef migrations add "orgaservice$(nowInMs)"
dotnet ef migrations bundle --self-contained --force
[ ! -f efbundle ] || ./efbundle --connection $ConnectionStrings__DefaultConnection


