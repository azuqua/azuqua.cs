# For building on unix
all:
	cd Azuqua && mcs -target:library -out:Azuqua.dll Azuqua.cs

.PHONY: test
test: all
	cd Test && mcs AzuquaTest.cs -r:../Azuqua/Azuqua.dll -r:nunit.framework.dll -target:library
