# For building on unix
all:
	cd Azuqua && mcs -target:library -out:Azuqua.dll MakeRequest.cs

.PHONY: test
test:
	cd Test && mcs AzuquaTest.cs
