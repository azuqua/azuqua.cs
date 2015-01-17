# For building on unix
all:
	cd Azuqua && mcs -target:library -out:Azuqua.dll Azuqua.cs

.PHONY: test
test:
	cd Test && mcs AzuquaTest.cs -r:../Azuqua/Azuqua.dll -r:nunit.framework.dll && mono AzuquaTest.exe
