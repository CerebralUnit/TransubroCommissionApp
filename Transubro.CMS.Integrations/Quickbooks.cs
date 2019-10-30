using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Interop.QBFC13;

namespace Transubro.CMS.Integrations
{
    public class Quickbooks
    {
        public void SavePurchaseOrder()
        {
             
        }


        public static void DoItemAdd()
        {
            bool sessionBegun = false;
            bool connectionOpen = false;
            QBSessionManager sessionManager = null;

            try
            {
                //Create the session Manager object
                sessionManager = new QBSessionManager();

                //Create the message set request object to hold our request
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 13, 0);
                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                BuildPurchaseOrderAddRq(requestMsgSet);

                //Connect to QuickBooks and begin a session
                sessionManager.OpenConnection("", "Sample Code from OSR");
                connectionOpen = true;
                sessionManager.BeginSession("", ENOpenMode.omDontCare);
                sessionBegun = true;

                //Send the request and get the response from QuickBooks
                IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);

                //End the session and close the connection to QuickBooks
                sessionManager.EndSession();
                sessionBegun = false;
                sessionManager.CloseConnection();
                connectionOpen = false;

                WalkPurchaseOrderAddRs(responseMsgSet);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
                if (sessionBegun)
                {
                    sessionManager.EndSession();
                }
                if (connectionOpen)
                {
                    sessionManager.CloseConnection();
                }
            }
        }

        public static void DoPurchaseOrderAdd()
        {
            bool sessionBegun = false;
            bool connectionOpen = false;
            QBSessionManager sessionManager = null;

            try
            {
                //Create the session Manager object
                sessionManager = new QBSessionManager();

                //Create the message set request object to hold our request
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 13, 0);
                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                BuildPurchaseOrderAddRq(requestMsgSet);

                //Connect to QuickBooks and begin a session
                sessionManager.OpenConnection("", "Sample Code from OSR");
                connectionOpen = true;
                sessionManager.BeginSession("", ENOpenMode.omDontCare);
                sessionBegun = true;

                //Send the request and get the response from QuickBooks
                IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);

                //End the session and close the connection to QuickBooks
                sessionManager.EndSession();
                sessionBegun = false;
                sessionManager.CloseConnection();
                connectionOpen = false;

                WalkPurchaseOrderAddRs(responseMsgSet);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error");
                if (sessionBegun)
                {
                    sessionManager.EndSession();
                }
                if (connectionOpen)
                {
                    sessionManager.CloseConnection();
                }
            }
        }

        static void BuildItemAddRq(IMsgSetRequest requestMsgSet)
        {  

        }

        static void BuildPurchaseOrderAddRq(IMsgSetRequest requestMsgSet)
        {
            IPurchaseOrderAdd PurchaseOrderAddRq = requestMsgSet.AppendPurchaseOrderAddRq();
            //Set attributes
            //Set field value for defMacro
            PurchaseOrderAddRq.defMacro.SetValue(" ");
            //Set field value for ListID
            PurchaseOrderAddRq.VendorRef.ListID.SetValue("200000-1011023419");
            //Set field value for FullName
            PurchaseOrderAddRq.VendorRef.FullName.SetValue("ab");
            //Set field value for ListID
            PurchaseOrderAddRq.ClassRef.ListID.SetValue("200000-1011023419");
            //Set field value for FullName
            PurchaseOrderAddRq.ClassRef.FullName.SetValue("ab");
            string ORInventorySiteORShipToEntityElementType17533 = "InventorySiteRef";
            if (ORInventorySiteORShipToEntityElementType17533 == "InventorySiteRef")
            {
                //Set field value for ListID
                PurchaseOrderAddRq.ORInventorySiteORShipToEntity.InventorySiteRef.ListID.SetValue("200000-1011023419");
                //Set field value for FullName
                PurchaseOrderAddRq.ORInventorySiteORShipToEntity.InventorySiteRef.FullName.SetValue("ab");
            }
            if (ORInventorySiteORShipToEntityElementType17533 == "ShipToEntityRef")
            {
                //Set field value for ListID
                PurchaseOrderAddRq.ORInventorySiteORShipToEntity.ShipToEntityRef.ListID.SetValue("200000-1011023419");
                //Set field value for FullName
                PurchaseOrderAddRq.ORInventorySiteORShipToEntity.ShipToEntityRef.FullName.SetValue("ab");
            }
            //Set field value for ListID
            PurchaseOrderAddRq.TemplateRef.ListID.SetValue("200000-1011023419");
            //Set field value for FullName
            PurchaseOrderAddRq.TemplateRef.FullName.SetValue("ab");
            //Set field value for TxnDate
            PurchaseOrderAddRq.TxnDate.SetValue(DateTime.Parse("12/15/2007"));
            //Set field value for RefNumber
            PurchaseOrderAddRq.RefNumber.SetValue("ab");
            //Set field value for Addr1
            PurchaseOrderAddRq.VendorAddress.Addr1.SetValue("ab");
            //Set field value for Addr2
            PurchaseOrderAddRq.VendorAddress.Addr2.SetValue("ab");
            //Set field value for Addr3
            PurchaseOrderAddRq.VendorAddress.Addr3.SetValue("ab");
            //Set field value for Addr4
            PurchaseOrderAddRq.VendorAddress.Addr4.SetValue("ab");
            //Set field value for Addr5
            PurchaseOrderAddRq.VendorAddress.Addr5.SetValue("ab");
            //Set field value for City
            PurchaseOrderAddRq.VendorAddress.City.SetValue("ab");
            //Set field value for State
            PurchaseOrderAddRq.VendorAddress.State.SetValue("ab");
            //Set field value for PostalCode
            PurchaseOrderAddRq.VendorAddress.PostalCode.SetValue("ab");
            //Set field value for Country
            PurchaseOrderAddRq.VendorAddress.Country.SetValue("ab");
            //Set field value for Note
            PurchaseOrderAddRq.VendorAddress.Note.SetValue("ab");
            //Set field value for Addr1
            PurchaseOrderAddRq.ShipAddress.Addr1.SetValue("ab");
            //Set field value for Addr2
            PurchaseOrderAddRq.ShipAddress.Addr2.SetValue("ab");
            //Set field value for Addr3
            PurchaseOrderAddRq.ShipAddress.Addr3.SetValue("ab");
            //Set field value for Addr4
            PurchaseOrderAddRq.ShipAddress.Addr4.SetValue("ab");
            //Set field value for Addr5
            PurchaseOrderAddRq.ShipAddress.Addr5.SetValue("ab");
            //Set field value for City
            PurchaseOrderAddRq.ShipAddress.City.SetValue("ab");
            //Set field value for State
            PurchaseOrderAddRq.ShipAddress.State.SetValue("ab");
            //Set field value for PostalCode
            PurchaseOrderAddRq.ShipAddress.PostalCode.SetValue("ab");
            //Set field value for Country
            PurchaseOrderAddRq.ShipAddress.Country.SetValue("ab");
            //Set field value for Note
            PurchaseOrderAddRq.ShipAddress.Note.SetValue("ab");
            //Set field value for ListID
            PurchaseOrderAddRq.TermsRef.ListID.SetValue("200000-1011023419");
            //Set field value for FullName
            PurchaseOrderAddRq.TermsRef.FullName.SetValue("ab");
            //Set field value for DueDate
            PurchaseOrderAddRq.DueDate.SetValue(DateTime.Parse("12/15/2007"));
            //Set field value for ExpectedDate
            PurchaseOrderAddRq.ExpectedDate.SetValue(DateTime.Parse("12/15/2007"));
            //Set field value for ListID
            PurchaseOrderAddRq.ShipMethodRef.ListID.SetValue("200000-1011023419");
            //Set field value for FullName
            PurchaseOrderAddRq.ShipMethodRef.FullName.SetValue("ab");
            //Set field value for FOB
            PurchaseOrderAddRq.FOB.SetValue("ab");
            //Set field value for Memo
            PurchaseOrderAddRq.Memo.SetValue("ab");
            //Set field value for VendorMsg
            PurchaseOrderAddRq.VendorMsg.SetValue("ab");
            //Set field value for IsToBePrinted
            PurchaseOrderAddRq.IsToBePrinted.SetValue(true);
            //Set field value for IsToBeEmailed
            PurchaseOrderAddRq.IsToBeEmailed.SetValue(true);
            //Set field value for IsTaxIncluded
            PurchaseOrderAddRq.IsTaxIncluded.SetValue(true);
            //Set field value for ListID
            PurchaseOrderAddRq.SalesTaxCodeRef.ListID.SetValue("200000-1011023419");
            //Set field value for FullName
            PurchaseOrderAddRq.SalesTaxCodeRef.FullName.SetValue("ab");
            //Set field value for Other1
            PurchaseOrderAddRq.Other1.SetValue("ab");
            //Set field value for Other2
            PurchaseOrderAddRq.Other2.SetValue("ab");
        
            //Set field value for ExternalGUID
            PurchaseOrderAddRq.ExternalGUID.SetValue(Guid.NewGuid().ToString());
            IORPurchaseOrderLineAdd ORPurchaseOrderLineAddListElement17534 = PurchaseOrderAddRq.ORPurchaseOrderLineAddList.Append();
            string ORPurchaseOrderLineAddListElementType17535 = "PurchaseOrderLineAdd";
            if (ORPurchaseOrderLineAddListElementType17535 == "PurchaseOrderLineAdd")
            {
                //Set attributes
                //Set field value for defMacro
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.defMacro.SetValue("IQBStringType");
                //Set field value for ListID
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.ItemRef.ListID.SetValue("200000-1011023419");
                //Set field value for FullName
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.ItemRef.FullName.SetValue("ab");
                //Set field value for ManufacturerPartNumber
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.ManufacturerPartNumber.SetValue("ab");
                //Set field value for Desc
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.Desc.SetValue("ab");
                //Set field value for Quantity
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.Quantity.SetValue(2);
                //Set field value for UnitOfMeasure
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.UnitOfMeasure.SetValue("ab");
                //Set field value for Rate
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.Rate.SetValue(15.65);
                //Set field value for ListID
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.ClassRef.ListID.SetValue("200000-1011023419");
                //Set field value for FullName
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.ClassRef.FullName.SetValue("ab");
                //Set field value for Amount
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.Amount.SetValue(10.01);
                //Set field value for ListID
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.InventorySiteLocationRef.ListID.SetValue("200000-1011023419");
                //Set field value for FullName
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.InventorySiteLocationRef.FullName.SetValue("ab");
                //Set field value for ListID
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.CustomerRef.ListID.SetValue("200000-1011023419");
                //Set field value for FullName
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.CustomerRef.FullName.SetValue("ab");
                //Set field value for ServiceDate
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.ServiceDate.SetValue(DateTime.Parse("12/15/2007"));
                //Set field value for ListID
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.SalesTaxCodeRef.ListID.SetValue("200000-1011023419");
                //Set field value for FullName
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.SalesTaxCodeRef.FullName.SetValue("ab");
                //Set field value for ListID
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.OverrideItemAccountRef.ListID.SetValue("200000-1011023419");
                //Set field value for FullName
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.OverrideItemAccountRef.FullName.SetValue("ab");
                //Set field value for Other1
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.Other1.SetValue("ab");
                //Set field value for Other2
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.Other2.SetValue("ab");
                IDataExt DataExt17536 = ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineAdd.DataExtList.Append();
                //Set field value for OwnerID
                DataExt17536.OwnerID.SetValue(Guid.NewGuid().ToString());
                //Set field value for DataExtName
                DataExt17536.DataExtName.SetValue("ab");
                //Set field value for DataExtValue
                DataExt17536.DataExtValue.SetValue("ab");
            }
            if (ORPurchaseOrderLineAddListElementType17535 == "PurchaseOrderLineGroupAdd")
            {
                //Set field value for ListID
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineGroupAdd.ItemGroupRef.ListID.SetValue("200000-1011023419");
                //Set field value for FullName
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineGroupAdd.ItemGroupRef.FullName.SetValue("ab");
                //Set field value for Quantity
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineGroupAdd.Quantity.SetValue(2);
                //Set field value for UnitOfMeasure
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineGroupAdd.UnitOfMeasure.SetValue("ab");
                //Set field value for ListID
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineGroupAdd.InventorySiteLocationRef.ListID.SetValue("200000-1011023419");
                //Set field value for FullName
                ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineGroupAdd.InventorySiteLocationRef.FullName.SetValue("ab");
                IDataExt DataExt17537 = ORPurchaseOrderLineAddListElement17534.PurchaseOrderLineGroupAdd.DataExtList.Append();
                //Set field value for OwnerID
                DataExt17537.OwnerID.SetValue(Guid.NewGuid().ToString());
                //Set field value for DataExtName
                DataExt17537.DataExtName.SetValue("ab");
                //Set field value for DataExtValue
                DataExt17537.DataExtValue.SetValue("ab");
            }
            //Set field value for IncludeRetElementList
            //May create more than one of these if needed
            PurchaseOrderAddRq.IncludeRetElementList.Add("ab");
        }




        static void WalkPurchaseOrderAddRs(IMsgSetResponse responseMsgSet)
        {
            if (responseMsgSet == null) return;
            IResponseList responseList = responseMsgSet.ResponseList;
            if (responseList == null) return;
            //if we sent only one request, there is only one response, we'll walk the list for this sample
            for (int i = 0; i < responseList.Count; i++)
            {
                IResponse response = responseList.GetAt(i);
                //check the status code of the response, 0=ok, >0 is warning
                if (response.StatusCode >= 0)
                {
                    //the request-specific response is in the details, make sure we have some
                    if (response.Detail != null)
                    {
                        //make sure the response is the type we're expecting
                        ENResponseType responseType = (ENResponseType)response.Type.GetValue();
                        if (responseType == ENResponseType.rtPurchaseOrderAddRs)
                        {
                            //upcast to more specific type here, this is safe because we checked with response.Type check above
                            IPurchaseOrderRet PurchaseOrderRet = (IPurchaseOrderRet)response.Detail;
                            WalkPurchaseOrderRet(PurchaseOrderRet);
                        }
                    }
                }
            }
        }




        static void WalkPurchaseOrderRet(IPurchaseOrderRet PurchaseOrderRet)
        {
            if (PurchaseOrderRet == null) return;
            //Go through all the elements of IPurchaseOrderRet
            //Get value of TxnID
            string TxnID17538 = (string)PurchaseOrderRet.TxnID.GetValue();
            //Get value of TimeCreated
            DateTime TimeCreated17539 = (DateTime)PurchaseOrderRet.TimeCreated.GetValue();
            //Get value of TimeModified
            DateTime TimeModified17540 = (DateTime)PurchaseOrderRet.TimeModified.GetValue();
            //Get value of EditSequence
            string EditSequence17541 = (string)PurchaseOrderRet.EditSequence.GetValue();
            //Get value of TxnNumber
            if (PurchaseOrderRet.TxnNumber != null)
            {
                int TxnNumber17542 = (int)PurchaseOrderRet.TxnNumber.GetValue();
            }
            if (PurchaseOrderRet.VendorRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.VendorRef.ListID != null)
                {
                    string ListID17543 = (string)PurchaseOrderRet.VendorRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.VendorRef.FullName != null)
                {
                    string FullName17544 = (string)PurchaseOrderRet.VendorRef.FullName.GetValue();
                }
            }
            if (PurchaseOrderRet.ClassRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.ClassRef.ListID != null)
                {
                    string ListID17545 = (string)PurchaseOrderRet.ClassRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.ClassRef.FullName != null)
                {
                    string FullName17546 = (string)PurchaseOrderRet.ClassRef.FullName.GetValue();
                }
            }
            if (PurchaseOrderRet.InventorySiteRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.InventorySiteRef.ListID != null)
                {
                    string ListID17547 = (string)PurchaseOrderRet.InventorySiteRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.InventorySiteRef.FullName != null)
                {
                    string FullName17548 = (string)PurchaseOrderRet.InventorySiteRef.FullName.GetValue();
                }
            }
            if (PurchaseOrderRet.ShipToEntityRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.ShipToEntityRef.ListID != null)
                {
                    string ListID17549 = (string)PurchaseOrderRet.ShipToEntityRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.ShipToEntityRef.FullName != null)
                {
                    string FullName17550 = (string)PurchaseOrderRet.ShipToEntityRef.FullName.GetValue();
                }
            }
            if (PurchaseOrderRet.TemplateRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.TemplateRef.ListID != null)
                {
                    string ListID17551 = (string)PurchaseOrderRet.TemplateRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.TemplateRef.FullName != null)
                {
                    string FullName17552 = (string)PurchaseOrderRet.TemplateRef.FullName.GetValue();
                }
            }
            //Get value of TxnDate
            DateTime TxnDate17553 = (DateTime)PurchaseOrderRet.TxnDate.GetValue();
            //Get value of RefNumber
            if (PurchaseOrderRet.RefNumber != null)
            {
                string RefNumber17554 = (string)PurchaseOrderRet.RefNumber.GetValue();
            }
            if (PurchaseOrderRet.VendorAddress != null)
            {
                //Get value of Addr1
                if (PurchaseOrderRet.VendorAddress.Addr1 != null)
                {
                    string Addr117555 = (string)PurchaseOrderRet.VendorAddress.Addr1.GetValue();
                }
                //Get value of Addr2
                if (PurchaseOrderRet.VendorAddress.Addr2 != null)
                {
                    string Addr217556 = (string)PurchaseOrderRet.VendorAddress.Addr2.GetValue();
                }
                //Get value of Addr3
                if (PurchaseOrderRet.VendorAddress.Addr3 != null)
                {
                    string Addr317557 = (string)PurchaseOrderRet.VendorAddress.Addr3.GetValue();
                }
                //Get value of Addr4
                if (PurchaseOrderRet.VendorAddress.Addr4 != null)
                {
                    string Addr417558 = (string)PurchaseOrderRet.VendorAddress.Addr4.GetValue();
                }
                //Get value of Addr5
                if (PurchaseOrderRet.VendorAddress.Addr5 != null)
                {
                    string Addr517559 = (string)PurchaseOrderRet.VendorAddress.Addr5.GetValue();
                }
                //Get value of City
                if (PurchaseOrderRet.VendorAddress.City != null)
                {
                    string City17560 = (string)PurchaseOrderRet.VendorAddress.City.GetValue();
                }
                //Get value of State
                if (PurchaseOrderRet.VendorAddress.State != null)
                {
                    string State17561 = (string)PurchaseOrderRet.VendorAddress.State.GetValue();
                }
                //Get value of PostalCode
                if (PurchaseOrderRet.VendorAddress.PostalCode != null)
                {
                    string PostalCode17562 = (string)PurchaseOrderRet.VendorAddress.PostalCode.GetValue();
                }
                //Get value of Country
                if (PurchaseOrderRet.VendorAddress.Country != null)
                {
                    string Country17563 = (string)PurchaseOrderRet.VendorAddress.Country.GetValue();
                }
                //Get value of Note
                if (PurchaseOrderRet.VendorAddress.Note != null)
                {
                    string Note17564 = (string)PurchaseOrderRet.VendorAddress.Note.GetValue();
                }
            }
            if (PurchaseOrderRet.VendorAddressBlock != null)
            {
                //Get value of Addr1
                if (PurchaseOrderRet.VendorAddressBlock.Addr1 != null)
                {
                    string Addr117565 = (string)PurchaseOrderRet.VendorAddressBlock.Addr1.GetValue();
                }
                //Get value of Addr2
                if (PurchaseOrderRet.VendorAddressBlock.Addr2 != null)
                {
                    string Addr217566 = (string)PurchaseOrderRet.VendorAddressBlock.Addr2.GetValue();
                }
                //Get value of Addr3
                if (PurchaseOrderRet.VendorAddressBlock.Addr3 != null)
                {
                    string Addr317567 = (string)PurchaseOrderRet.VendorAddressBlock.Addr3.GetValue();
                }
                //Get value of Addr4
                if (PurchaseOrderRet.VendorAddressBlock.Addr4 != null)
                {
                    string Addr417568 = (string)PurchaseOrderRet.VendorAddressBlock.Addr4.GetValue();
                }
                //Get value of Addr5
                if (PurchaseOrderRet.VendorAddressBlock.Addr5 != null)
                {
                    string Addr517569 = (string)PurchaseOrderRet.VendorAddressBlock.Addr5.GetValue();
                }
            }
            if (PurchaseOrderRet.ShipAddress != null)
            {
                //Get value of Addr1
                if (PurchaseOrderRet.ShipAddress.Addr1 != null)
                {
                    string Addr117570 = (string)PurchaseOrderRet.ShipAddress.Addr1.GetValue();
                }
                //Get value of Addr2
                if (PurchaseOrderRet.ShipAddress.Addr2 != null)
                {
                    string Addr217571 = (string)PurchaseOrderRet.ShipAddress.Addr2.GetValue();
                }
                //Get value of Addr3
                if (PurchaseOrderRet.ShipAddress.Addr3 != null)
                {
                    string Addr317572 = (string)PurchaseOrderRet.ShipAddress.Addr3.GetValue();
                }
                //Get value of Addr4
                if (PurchaseOrderRet.ShipAddress.Addr4 != null)
                {
                    string Addr417573 = (string)PurchaseOrderRet.ShipAddress.Addr4.GetValue();
                }
                //Get value of Addr5
                if (PurchaseOrderRet.ShipAddress.Addr5 != null)
                {
                    string Addr517574 = (string)PurchaseOrderRet.ShipAddress.Addr5.GetValue();
                }
                //Get value of City
                if (PurchaseOrderRet.ShipAddress.City != null)
                {
                    string City17575 = (string)PurchaseOrderRet.ShipAddress.City.GetValue();
                }
                //Get value of State
                if (PurchaseOrderRet.ShipAddress.State != null)
                {
                    string State17576 = (string)PurchaseOrderRet.ShipAddress.State.GetValue();
                }
                //Get value of PostalCode
                if (PurchaseOrderRet.ShipAddress.PostalCode != null)
                {
                    string PostalCode17577 = (string)PurchaseOrderRet.ShipAddress.PostalCode.GetValue();
                }
                //Get value of Country
                if (PurchaseOrderRet.ShipAddress.Country != null)
                {
                    string Country17578 = (string)PurchaseOrderRet.ShipAddress.Country.GetValue();
                }
                //Get value of Note
                if (PurchaseOrderRet.ShipAddress.Note != null)
                {
                    string Note17579 = (string)PurchaseOrderRet.ShipAddress.Note.GetValue();
                }
            }
            if (PurchaseOrderRet.ShipAddressBlock != null)
            {
                //Get value of Addr1
                if (PurchaseOrderRet.ShipAddressBlock.Addr1 != null)
                {
                    string Addr117580 = (string)PurchaseOrderRet.ShipAddressBlock.Addr1.GetValue();
                }
                //Get value of Addr2
                if (PurchaseOrderRet.ShipAddressBlock.Addr2 != null)
                {
                    string Addr217581 = (string)PurchaseOrderRet.ShipAddressBlock.Addr2.GetValue();
                }
                //Get value of Addr3
                if (PurchaseOrderRet.ShipAddressBlock.Addr3 != null)
                {
                    string Addr317582 = (string)PurchaseOrderRet.ShipAddressBlock.Addr3.GetValue();
                }
                //Get value of Addr4
                if (PurchaseOrderRet.ShipAddressBlock.Addr4 != null)
                {
                    string Addr417583 = (string)PurchaseOrderRet.ShipAddressBlock.Addr4.GetValue();
                }
                //Get value of Addr5
                if (PurchaseOrderRet.ShipAddressBlock.Addr5 != null)
                {
                    string Addr517584 = (string)PurchaseOrderRet.ShipAddressBlock.Addr5.GetValue();
                }
            }
            if (PurchaseOrderRet.TermsRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.TermsRef.ListID != null)
                {
                    string ListID17585 = (string)PurchaseOrderRet.TermsRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.TermsRef.FullName != null)
                {
                    string FullName17586 = (string)PurchaseOrderRet.TermsRef.FullName.GetValue();
                }
            }
            //Get value of DueDate
            if (PurchaseOrderRet.DueDate != null)
            {
                DateTime DueDate17587 = (DateTime)PurchaseOrderRet.DueDate.GetValue();
            }
            //Get value of ExpectedDate
            if (PurchaseOrderRet.ExpectedDate != null)
            {
                DateTime ExpectedDate17588 = (DateTime)PurchaseOrderRet.ExpectedDate.GetValue();
            }
            if (PurchaseOrderRet.ShipMethodRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.ShipMethodRef.ListID != null)
                {
                    string ListID17589 = (string)PurchaseOrderRet.ShipMethodRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.ShipMethodRef.FullName != null)
                {
                    string FullName17590 = (string)PurchaseOrderRet.ShipMethodRef.FullName.GetValue();
                }
            }
            //Get value of FOB
            if (PurchaseOrderRet.FOB != null)
            {
                string FOB17591 = (string)PurchaseOrderRet.FOB.GetValue();
            }
            //Get value of TotalAmount
            if (PurchaseOrderRet.TotalAmount != null)
            {
                double TotalAmount17592 = (double)PurchaseOrderRet.TotalAmount.GetValue();
            }
            if (PurchaseOrderRet.CurrencyRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.CurrencyRef.ListID != null)
                {
                    string ListID17593 = (string)PurchaseOrderRet.CurrencyRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.CurrencyRef.FullName != null)
                {
                    string FullName17594 = (string)PurchaseOrderRet.CurrencyRef.FullName.GetValue();
                }
            }
            
            //Get value of TotalAmountInHomeCurrency
            if (PurchaseOrderRet.TotalAmountInHomeCurrency != null)
            {
                double TotalAmountInHomeCurrency17596 = (double)PurchaseOrderRet.TotalAmountInHomeCurrency.GetValue();
            }
            //Get value of IsManuallyClosed
            if (PurchaseOrderRet.IsManuallyClosed != null)
            {
                bool IsManuallyClosed17597 = (bool)PurchaseOrderRet.IsManuallyClosed.GetValue();
            }
            //Get value of IsFullyReceived
            if (PurchaseOrderRet.IsFullyReceived != null)
            {
                bool IsFullyReceived17598 = (bool)PurchaseOrderRet.IsFullyReceived.GetValue();
            }
            //Get value of Memo
            if (PurchaseOrderRet.Memo != null)
            {
                string Memo17599 = (string)PurchaseOrderRet.Memo.GetValue();
            }
            //Get value of VendorMsg
            if (PurchaseOrderRet.VendorMsg != null)
            {
                string VendorMsg17600 = (string)PurchaseOrderRet.VendorMsg.GetValue();
            }
            //Get value of IsToBePrinted
            if (PurchaseOrderRet.IsToBePrinted != null)
            {
                bool IsToBePrinted17601 = (bool)PurchaseOrderRet.IsToBePrinted.GetValue();
            }
            //Get value of IsToBeEmailed
            if (PurchaseOrderRet.IsToBeEmailed != null)
            {
                bool IsToBeEmailed17602 = (bool)PurchaseOrderRet.IsToBeEmailed.GetValue();
            }
            //Get value of IsTaxIncluded
            if (PurchaseOrderRet.IsTaxIncluded != null)
            {
                bool IsTaxIncluded17603 = (bool)PurchaseOrderRet.IsTaxIncluded.GetValue();
            }
            if (PurchaseOrderRet.SalesTaxCodeRef != null)
            {
                //Get value of ListID
                if (PurchaseOrderRet.SalesTaxCodeRef.ListID != null)
                {
                    string ListID17604 = (string)PurchaseOrderRet.SalesTaxCodeRef.ListID.GetValue();
                }
                //Get value of FullName
                if (PurchaseOrderRet.SalesTaxCodeRef.FullName != null)
                {
                    string FullName17605 = (string)PurchaseOrderRet.SalesTaxCodeRef.FullName.GetValue();
                }
            }
            //Get value of Other1
            if (PurchaseOrderRet.Other1 != null)
            {
                string Other117606 = (string)PurchaseOrderRet.Other1.GetValue();
            }
            //Get value of Other2
            if (PurchaseOrderRet.Other2 != null)
            {
                string Other217607 = (string)PurchaseOrderRet.Other2.GetValue();
            }
            //Get value of ExternalGUID
            if (PurchaseOrderRet.ExternalGUID != null)
            {
                string ExternalGUID17608 = (string)PurchaseOrderRet.ExternalGUID.GetValue();
            }
            if (PurchaseOrderRet.LinkedTxnList != null)
            {
                for (int i17609 = 0; i17609 < PurchaseOrderRet.LinkedTxnList.Count; i17609++)
                {
                    ILinkedTxn LinkedTxn = PurchaseOrderRet.LinkedTxnList.GetAt(i17609);
                    //Get value of TxnID
                    string TxnID17610 = (string)LinkedTxn.TxnID.GetValue();
                    //Get value of TxnType
                    ENTxnType TxnType17611 = (ENTxnType)LinkedTxn.TxnType.GetValue();
                    //Get value of TxnDate
                    DateTime TxnDate17612 = (DateTime)LinkedTxn.TxnDate.GetValue();
                    //Get value of RefNumber
                    if (LinkedTxn.RefNumber != null)
                    {
                        string RefNumber17613 = (string)LinkedTxn.RefNumber.GetValue();
                    }
                    //Get value of LinkType
                    if (LinkedTxn.LinkType != null)
                    {
                        ENLinkType LinkType17614 = (ENLinkType)LinkedTxn.LinkType.GetValue();
                    }
                    //Get value of Amount
                    double Amount17615 = (double)LinkedTxn.Amount.GetValue();
                }
            }
            if (PurchaseOrderRet.ORPurchaseOrderLineRetList != null)
            {
                for (int i17616 = 0; i17616 < PurchaseOrderRet.ORPurchaseOrderLineRetList.Count; i17616++)
                {
                    IORPurchaseOrderLineRet ORPurchaseOrderLineRet17617 = PurchaseOrderRet.ORPurchaseOrderLineRetList.GetAt(i17616);
                    if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet != null)
                    {
                        if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet != null)
                        {
                            //Get value of TxnLineID
                            string TxnLineID17618 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.TxnLineID.GetValue();
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ItemRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ItemRef.ListID != null)
                                {
                                    string ListID17619 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ItemRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ItemRef.FullName != null)
                                {
                                    string FullName17620 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ItemRef.FullName.GetValue();
                                }
                            }
                            //Get value of ManufacturerPartNumber
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ManufacturerPartNumber != null)
                            {
                                string ManufacturerPartNumber17621 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ManufacturerPartNumber.GetValue();
                            }
                            //Get value of Desc
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Desc != null)
                            {
                                string Desc17622 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Desc.GetValue();
                            }
                            //Get value of Quantity
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Quantity != null)
                            {
                                int Quantity17623 = (int)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Quantity.GetValue();
                            }
                            //Get value of UnitOfMeasure
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.UnitOfMeasure != null)
                            {
                                string UnitOfMeasure17624 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.UnitOfMeasure.GetValue();
                            }
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.OverrideUOMSetRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.OverrideUOMSetRef.ListID != null)
                                {
                                    string ListID17625 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.OverrideUOMSetRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.OverrideUOMSetRef.FullName != null)
                                {
                                    string FullName17626 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.OverrideUOMSetRef.FullName.GetValue();
                                }
                            }
                            //Get value of Rate
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Rate != null)
                            {
                                double Rate17627 = (double)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Rate.GetValue();
                            }
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ClassRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ClassRef.ListID != null)
                                {
                                    string ListID17628 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ClassRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ClassRef.FullName != null)
                                {
                                    string FullName17629 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ClassRef.FullName.GetValue();
                                }
                            }
                            //Get value of Amount
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Amount != null)
                            {
                                double Amount17630 = (double)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Amount.GetValue();
                            }
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.InventorySiteLocationRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.InventorySiteLocationRef.ListID != null)
                                {
                                    string ListID17631 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.InventorySiteLocationRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.InventorySiteLocationRef.FullName != null)
                                {
                                    string FullName17632 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.InventorySiteLocationRef.FullName.GetValue();
                                }
                            }
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.CustomerRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.CustomerRef.ListID != null)
                                {
                                    string ListID17633 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.CustomerRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.CustomerRef.FullName != null)
                                {
                                    string FullName17634 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.CustomerRef.FullName.GetValue();
                                }
                            }
                            //Get value of ServiceDate
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ServiceDate != null)
                            {
                                DateTime ServiceDate17635 = (DateTime)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ServiceDate.GetValue();
                            }
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.SalesTaxCodeRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.SalesTaxCodeRef.ListID != null)
                                {
                                    string ListID17636 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.SalesTaxCodeRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.SalesTaxCodeRef.FullName != null)
                                {
                                    string FullName17637 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.SalesTaxCodeRef.FullName.GetValue();
                                }
                            }
                            //Get value of ReceivedQuantity
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ReceivedQuantity != null)
                            {
                                int ReceivedQuantity17638 = (int)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.ReceivedQuantity.GetValue();
                            }
                            //Get value of UnbilledQuantity
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.UnbilledQuantity != null)
                            {
                                int UnbilledQuantity17639 = (int)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.UnbilledQuantity.GetValue();
                            }
                            //Get value of IsBilled
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.IsBilled != null)
                            {
                                bool IsBilled17640 = (bool)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.IsBilled.GetValue();
                            }
                            //Get value of IsManuallyClosed
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.IsManuallyClosed != null)
                            {
                                bool IsManuallyClosed17641 = (bool)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.IsManuallyClosed.GetValue();
                            }
                            //Get value of Other1
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Other1 != null)
                            {
                                string Other117642 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Other1.GetValue();
                            }
                            //Get value of Other2
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Other2 != null)
                            {
                                string Other217643 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.Other2.GetValue();
                            }
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.DataExtRetList != null)
                            {
                                for (int i17644 = 0; i17644 < ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.DataExtRetList.Count; i17644++)
                                {
                                    IDataExtRet DataExtRet = ORPurchaseOrderLineRet17617.PurchaseOrderLineRet.DataExtRetList.GetAt(i17644);
                                    //Get value of OwnerID
                                    if (DataExtRet.OwnerID != null)
                                    {
                                        string OwnerID17645 = (string)DataExtRet.OwnerID.GetValue();
                                    }
                                    //Get value of DataExtName
                                    string DataExtName17646 = (string)DataExtRet.DataExtName.GetValue();
                                    //Get value of DataExtType
                                    ENDataExtType DataExtType17647 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                                    //Get value of DataExtValue
                                    string DataExtValue17648 = (string)DataExtRet.DataExtValue.GetValue();
                                }
                            }
                        }
                    }
                    if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet != null)
                    {
                        if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet != null)
                        {
                            //Get value of TxnLineID
                            string TxnLineID17649 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.TxnLineID.GetValue();
                            //Get value of ListID
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.ItemGroupRef.ListID != null)
                            {
                                string ListID17650 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.ItemGroupRef.ListID.GetValue();
                            }
                            //Get value of FullName
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.ItemGroupRef.FullName != null)
                            {
                                string FullName17651 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.ItemGroupRef.FullName.GetValue();
                            }
                            //Get value of Desc
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.Desc != null)
                            {
                                string Desc17652 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.Desc.GetValue();
                            }
                            //Get value of Quantity
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.Quantity != null)
                            {
                                int Quantity17653 = (int)ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.Quantity.GetValue();
                            }
                            //Get value of UnitOfMeasure
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.UnitOfMeasure != null)
                            {
                                string UnitOfMeasure17654 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.UnitOfMeasure.GetValue();
                            }
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.OverrideUOMSetRef != null)
                            {
                                //Get value of ListID
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.OverrideUOMSetRef.ListID != null)
                                {
                                    string ListID17655 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.OverrideUOMSetRef.ListID.GetValue();
                                }
                                //Get value of FullName
                                if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.OverrideUOMSetRef.FullName != null)
                                {
                                    string FullName17656 = (string)ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.OverrideUOMSetRef.FullName.GetValue();
                                }
                            }
                            //Get value of IsPrintItemsInGroup
                            bool IsPrintItemsInGroup17657 = (bool)ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.IsPrintItemsInGroup.GetValue();
                            //Get value of TotalAmount
                            double TotalAmount17658 = (double)ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.TotalAmount.GetValue();
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.PurchaseOrderLineRetList != null)
                            {
                                for (int i17659 = 0; i17659 < ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.PurchaseOrderLineRetList.Count; i17659++)
                                {
                                    IPurchaseOrderLineRet PurchaseOrderLineRet = ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.PurchaseOrderLineRetList.GetAt(i17659);
                                    //Get value of TxnLineID
                                    string TxnLineID17660 = (string)PurchaseOrderLineRet.TxnLineID.GetValue();
                                    if (PurchaseOrderLineRet.ItemRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.ItemRef.ListID != null)
                                        {
                                            string ListID17661 = (string)PurchaseOrderLineRet.ItemRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.ItemRef.FullName != null)
                                        {
                                            string FullName17662 = (string)PurchaseOrderLineRet.ItemRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of ManufacturerPartNumber
                                    if (PurchaseOrderLineRet.ManufacturerPartNumber != null)
                                    {
                                        string ManufacturerPartNumber17663 = (string)PurchaseOrderLineRet.ManufacturerPartNumber.GetValue();
                                    }
                                    //Get value of Desc
                                    if (PurchaseOrderLineRet.Desc != null)
                                    {
                                        string Desc17664 = (string)PurchaseOrderLineRet.Desc.GetValue();
                                    }
                                    //Get value of Quantity
                                    if (PurchaseOrderLineRet.Quantity != null)
                                    {
                                        int Quantity17665 = (int)PurchaseOrderLineRet.Quantity.GetValue();
                                    }
                                    //Get value of UnitOfMeasure
                                    if (PurchaseOrderLineRet.UnitOfMeasure != null)
                                    {
                                        string UnitOfMeasure17666 = (string)PurchaseOrderLineRet.UnitOfMeasure.GetValue();
                                    }
                                    if (PurchaseOrderLineRet.OverrideUOMSetRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.OverrideUOMSetRef.ListID != null)
                                        {
                                            string ListID17667 = (string)PurchaseOrderLineRet.OverrideUOMSetRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.OverrideUOMSetRef.FullName != null)
                                        {
                                            string FullName17668 = (string)PurchaseOrderLineRet.OverrideUOMSetRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of Rate
                                    if (PurchaseOrderLineRet.Rate != null)
                                    {
                                        double Rate17669 = (double)PurchaseOrderLineRet.Rate.GetValue();
                                    }
                                    if (PurchaseOrderLineRet.ClassRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.ClassRef.ListID != null)
                                        {
                                            string ListID17670 = (string)PurchaseOrderLineRet.ClassRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.ClassRef.FullName != null)
                                        {
                                            string FullName17671 = (string)PurchaseOrderLineRet.ClassRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of Amount
                                    if (PurchaseOrderLineRet.Amount != null)
                                    {
                                        double Amount17672 = (double)PurchaseOrderLineRet.Amount.GetValue();
                                    }
                                    if (PurchaseOrderLineRet.InventorySiteLocationRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.InventorySiteLocationRef.ListID != null)
                                        {
                                            string ListID17673 = (string)PurchaseOrderLineRet.InventorySiteLocationRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.InventorySiteLocationRef.FullName != null)
                                        {
                                            string FullName17674 = (string)PurchaseOrderLineRet.InventorySiteLocationRef.FullName.GetValue();
                                        }
                                    }
                                    if (PurchaseOrderLineRet.CustomerRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.CustomerRef.ListID != null)
                                        {
                                            string ListID17675 = (string)PurchaseOrderLineRet.CustomerRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.CustomerRef.FullName != null)
                                        {
                                            string FullName17676 = (string)PurchaseOrderLineRet.CustomerRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of ServiceDate
                                    if (PurchaseOrderLineRet.ServiceDate != null)
                                    {
                                        DateTime ServiceDate17677 = (DateTime)PurchaseOrderLineRet.ServiceDate.GetValue();
                                    }
                                    if (PurchaseOrderLineRet.SalesTaxCodeRef != null)
                                    {
                                        //Get value of ListID
                                        if (PurchaseOrderLineRet.SalesTaxCodeRef.ListID != null)
                                        {
                                            string ListID17678 = (string)PurchaseOrderLineRet.SalesTaxCodeRef.ListID.GetValue();
                                        }
                                        //Get value of FullName
                                        if (PurchaseOrderLineRet.SalesTaxCodeRef.FullName != null)
                                        {
                                            string FullName17679 = (string)PurchaseOrderLineRet.SalesTaxCodeRef.FullName.GetValue();
                                        }
                                    }
                                    //Get value of ReceivedQuantity
                                    if (PurchaseOrderLineRet.ReceivedQuantity != null)
                                    {
                                        int ReceivedQuantity17680 = (int)PurchaseOrderLineRet.ReceivedQuantity.GetValue();
                                    }
                                    //Get value of UnbilledQuantity
                                    if (PurchaseOrderLineRet.UnbilledQuantity != null)
                                    {
                                        int UnbilledQuantity17681 = (int)PurchaseOrderLineRet.UnbilledQuantity.GetValue();
                                    }
                                    //Get value of IsBilled
                                    if (PurchaseOrderLineRet.IsBilled != null)
                                    {
                                        bool IsBilled17682 = (bool)PurchaseOrderLineRet.IsBilled.GetValue();
                                    }
                                    //Get value of IsManuallyClosed
                                    if (PurchaseOrderLineRet.IsManuallyClosed != null)
                                    {
                                        bool IsManuallyClosed17683 = (bool)PurchaseOrderLineRet.IsManuallyClosed.GetValue();
                                    }
                                    //Get value of Other1
                                    if (PurchaseOrderLineRet.Other1 != null)
                                    {
                                        string Other117684 = (string)PurchaseOrderLineRet.Other1.GetValue();
                                    }
                                    //Get value of Other2
                                    if (PurchaseOrderLineRet.Other2 != null)
                                    {
                                        string Other217685 = (string)PurchaseOrderLineRet.Other2.GetValue();
                                    }
                                    if (PurchaseOrderLineRet.DataExtRetList != null)
                                    {
                                        for (int i17686 = 0; i17686 < PurchaseOrderLineRet.DataExtRetList.Count; i17686++)
                                        {
                                            IDataExtRet DataExtRet = PurchaseOrderLineRet.DataExtRetList.GetAt(i17686);
                                            //Get value of OwnerID
                                            if (DataExtRet.OwnerID != null)
                                            {
                                                string OwnerID17687 = (string)DataExtRet.OwnerID.GetValue();
                                            }
                                            //Get value of DataExtName
                                            string DataExtName17688 = (string)DataExtRet.DataExtName.GetValue();
                                            //Get value of DataExtType
                                            ENDataExtType DataExtType17689 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                                            //Get value of DataExtValue
                                            string DataExtValue17690 = (string)DataExtRet.DataExtValue.GetValue();
                                        }
                                    }
                                }
                            }
                            if (ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.DataExtRetList != null)
                            {
                                for (int i17691 = 0; i17691 < ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.DataExtRetList.Count; i17691++)
                                {
                                    IDataExtRet DataExtRet = ORPurchaseOrderLineRet17617.PurchaseOrderLineGroupRet.DataExtRetList.GetAt(i17691);
                                    //Get value of OwnerID
                                    if (DataExtRet.OwnerID != null)
                                    {
                                        string OwnerID17692 = (string)DataExtRet.OwnerID.GetValue();
                                    }
                                    //Get value of DataExtName
                                    string DataExtName17693 = (string)DataExtRet.DataExtName.GetValue();
                                    //Get value of DataExtType
                                    ENDataExtType DataExtType17694 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                                    //Get value of DataExtValue
                                    string DataExtValue17695 = (string)DataExtRet.DataExtValue.GetValue();
                                }
                            }
                        }
                    }
                }
            }
            if (PurchaseOrderRet.DataExtRetList != null)
            {
                for (int i17696 = 0; i17696 < PurchaseOrderRet.DataExtRetList.Count; i17696++)
                {
                    IDataExtRet DataExtRet = PurchaseOrderRet.DataExtRetList.GetAt(i17696);
                    //Get value of OwnerID
                    if (DataExtRet.OwnerID != null)
                    {
                        string OwnerID17697 = (string)DataExtRet.OwnerID.GetValue();
                    }
                    //Get value of DataExtName
                    string DataExtName17698 = (string)DataExtRet.DataExtName.GetValue();
                    //Get value of DataExtType
                    ENDataExtType DataExtType17699 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                    //Get value of DataExtValue
                    string DataExtValue17700 = (string)DataExtRet.DataExtValue.GetValue();
                }
            }
        }
    }
}
