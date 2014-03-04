AvaTax-REST-csharp
=====================

This is a C# sample demonstrating the [AvaTax REST API](http://developer.avalara.com/api-docs/rest) methods:
 [tax/get POST](http://developer.avalara.com/api-docs/rest/tax/post/), [tax/get GET](http://developer.avalara.com/api-docs/rest/tax/get), [tax/cancel POST](http://developer.avalara.com/api-docs/rest/tax/cancel), and [address/validate GET](http://developer.avalara.com/api-docs/rest/address-validation).
 
 For more information on the use of these methods and the AvaTax product, please visit our [developer site](http://developer.avalara.com/) or [homepage](http://www.avalara.com/)
 
Contents:
----------
 
<table>
<tr><td>Sample Files</td></tr>
<tr><td>CancelTaxTest.cs</td><td>Demonstrates the [CancelTax](http://developer.avalara.com/api-docs/rest/tax/cancel) method used to [void a document](http://developer.avalara.com/api-docs/api-reference/canceltax).</td></tr>
<tr><td>EstimateTaxTest.cs</td><td>Demonstrates the [EstimateTax](http://developer.avalara.com/api-docs/rest/tax/get) method used for product- and line- indifferent tax estimates.</td></tr>
<tr><td>GetTaxTest.cs</td><td>Demonstrates the [GetTax](http://developer.avalara.com/api-docs/rest/tax/post) method used for product- and line- specific [calculation](http://developer.avalara.com/api-docs/api-reference/gettax).</td></tr>
<tr><td>PingTest.cs</td><td>Uses a hardcoded EstimateTax call to test connectivity and credential information.</td></tr>
<tr><td>Program.cs</td><td>Provides and entry point to call the actual samples.</td></tr>
<tr><td>ValidateAddressTest.cs</td><td>Demonstrates the [ValidateAddress](http://developer.avalara.com/api-docs/rest/address-validation) method to [normalize an address](http://developer.avalara.com/api-docs/api-reference/address-validation).</td></tr>
<tr><td>Core Classes</td></tr>
<tr><td>AddressSvc.cs</td><td>Contains the necessary classes for ValidateAddress.</td></tr>
<tr><td>CommonResponse.cs</td><td>Contains classes necessary for and common to both address and tax methods.</td></tr>
<tr><td>TaxSvc.cs</td><td>Contains the necessary classes for CancelTax, EstimateTax, GetTax, and Ping.</td></tr> 
<tr><td>Other Files</td></tr>
<tr><td>Properties/AssemblyInfo.cs</td><td>-</td></tr>
<tr><td>.gitattributes</td><td>-</td></tr>
<tr><td>.gitignore</td><td>-</td></tr>
<tr><td>AvaTaxCalcREST.csproj</td><td>-</td></tr>
<tr><td>AvaTaxCalcREST.sln</td><td>-</td></tr>
<tr><td>LICENSE.md</td><td>-</td></tr>
<tr><td>README.md</td><td>-</td></tr>
</table>

Dependencies:
-----------
- .NET 4.0 or later


Requirements:
----------
- Authentication requires an valid **Account Number** and **License Key**, which should be entered in the test file (e.g. GetTaxTest.cs) you would like to run.
- If you do not have an AvaTax account, a free trial account can be acquired through our [developer site](http://developer.avalara.com/api-get-started)
 