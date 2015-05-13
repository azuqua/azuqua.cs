# For building on unix
all:
	mcs -target:library -out:Azuqua.dll -r:./Newtonsoft.Json.dll Azuqua/Azuqua.cs

buildtest: all
	mcs AzuquaTest.cs -r:Azuqua.dll,nunit.framework.dll -target:library

.PHONY: test
test: buildtest
	nunit-console AzuquaTest.dll
