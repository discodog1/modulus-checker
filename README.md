# Modulus Checker

A C# Web API to validate bank account numbers against sort codes. (Built on .Net Core 2.1).

Currently handles exceptions 4 and 7, other identified exceptions are referenced in the `ResultMessage` property of the output model.

# Usage

Post JSON model to `/api/Modulus/`, i.e.

~~~~
{
	"sortCode": "870000",
	"accountNumber": "63748472"
}
~~~~

to receive a JSON object with the results:

~~~~
{
    "CanValidate": true,
    "PassedValidation": false,
    "ResultMessage": "First Exception is 10 which has not been handled in this test.
                      Second Exception is 11 which has not been handled in this test",
    "FirstAlgorithm": "MOD11",
    "FirstCheckIsException4": false,
    "FirstCheckIsException7": false,
    "PassedFirstCheck": false,
    "RequiresSecondCheck": true,
    "PassedSecondCheck": true,
    "SecondCheckIsException4": false,
    "SecondCheckIsException7": false,
    "SecondAlgorithm": "MOD11"
}
~~~~

# Testing

A test project has been included which should cover all of the functionality requested.
