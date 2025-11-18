./start_test_containers.sh
cd ./Backend/test/XratesTests/
dotnet test -v diag --collect:"XPlat Code Coverage" --settings ../../../coverlet.runsettings
cd -
./stop_test_containers.sh
