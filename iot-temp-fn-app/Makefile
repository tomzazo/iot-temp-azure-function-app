.PHONY: build publish
.SILENT: build publish

build:
	dotnet build

publish:
	func azure functionapp publish iot-temp-fn-app --csharp
