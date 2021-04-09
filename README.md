# CASInterfaceService
An API for submitting and requesting specific types of transaction data from CAS.

API endpoints:
* **api/CASAPTransaction** - Submit a transaction (payment request) to CAS.
* **api/CASAPRetrieve/GetTransactionRecords** - Request details for a specific transaction. Despite name, does not accept batch requests.

These requests will not go through without CAS-approved authentication headers.