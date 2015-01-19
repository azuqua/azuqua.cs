# For building on unix
all:
	mcs -target:library -out:Azuqua.dll Azuqua/Azuqua.cs

buildtest: all
	mcs Test/AzuquaTest.cs -r:Azuqua.dll,nunit.framework.dll -target:library

.PHONY: test
test: buildtest
	cd Test && nunit-console AzuquaTest.dll
