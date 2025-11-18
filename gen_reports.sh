dir="$PWD/Backend/test/XratesTests/TestResults/*/coverage.cobertura.xml"
reportgenerator \
-reports:"$dir" \
-targetdir:"coveragereport" \
-reporttypes:Html \
-classfilters:"-Xrates.Migrations*,-Program"

firefox ./coveragereport/index.html
