Feature: JsonPortableModelBinder
	Allows to get instances out of binary requests like HTTP or TCP

@infrastructure
Scenario: Deserialize a simple json content
	Given A JSON Portable model binder
	And An encoding of "utf-8"
	And A json content
	And The encoded binary content of the json
	When The binary content is deserialized
	Then The bound object is the expected one
