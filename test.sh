./start_test_containers.sh
cd ./Backend/test/XratesTests/
rm -f ./TestResults/*/coverage.cobertura.xml
dotnet test -v diag --collect:"XPlat Code Coverage" --settings ../../../coverlet.runsettings
cd -
./stop_test_containers.sh
