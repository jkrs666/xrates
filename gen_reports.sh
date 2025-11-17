reportgenerator \
-reports:"/home/satyr/Desktop/archive/archive/repos/k2m_assesment/Backend/test/XratesTests/TestResults/*/coverage.cobertura.xml" \
-targetdir:"coveragereport" \
-reporttypes:Html \
-classfilters:"-Xrates.Migrations*"

firefox ./coveragereport/index.html
