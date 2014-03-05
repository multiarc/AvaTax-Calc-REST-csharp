AvaTax-REST-csharp
=====================

This is a C# sample demonstrating the [AvaTax REST API](http://developer.avalara.com/api-docs/rest) methods:
 [tax/get POST](http://developer.avalara.com/api-docs/rest/tax/post/), [tax/get GET](http://developer.avalara.com/api-docs/rest/tax/get), [tax/cancel POST](http://developer.avalara.com/api-docs/rest/tax/cancel), and [address/validate GET](http://developer.avalara.com/api-docs/rest/address-validation).
 
 For more information on the use of these methods and the AvaTax product, please visit our [developer site](http://developer.avalara.com/) or [homepage](http://www.avalara.com/)
 
Contents:
----------
 
<table>
<th colspan="2" align=left>Sample Files</th>
<tr><td>CancelTaxTest.cs</td><td>Demonstrates the <a href="http://developer.avalara.com/api-docs/rest/tax/cancel">CancelTax</a> method used to <a href="http://developer.avalara.com/api-docs/api-reference/canceltax">void a document</a>.</td></tr>
<tr><td>EstimateTaxTest.cs</td><td>Demonstrates the <a href="http://developer.avalara.com/api-docs/rest/tax/get">EstimateTax</a> method used for product- and line- indifferent tax estimates.</td></tr>
<tr><td>GetTaxTest.cs</td><td>Demonstrates the <a href="http://developer.avalara.com/api-docs/rest/tax/post">GetTax</a> method used for product- and line- specific <a href="http://developer.avalara.com/api-docs/api-reference/gettax">calculation</a>.</td></tr>
<tr><td>PingTest.cs</td><td>Uses a hardcoded EstimateTax call to test connectivity and credential information.</td></tr>
<tr><td>Program.cs</td><td>Provides and entry point to call the actual samples.</td></tr>
<tr><td>ValidateAddressTest.cs</td><td>Demonstrates the <a href="http://developer.avalara.com/api-docs/rest/address-validation">ValidateAddress</a> method to <a href="http://developer.avalara.com/api-docs/api-reference/address-validation">normalize an address</a>.</td></tr>
<th colspan="2" align=left>Other Files</th>
<tr><td>AvaTaxClasses/</td><td>Contains the core classes that make the service calls.</td></tr>
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
 