#if UNITY_ANDROID || UNITY_IPHONE
// You must obfuscate your secrets using Window > Unity IAP > Receipt Validation Obfuscator
// before receipt validation will compile in this sample.
#define RECEIPT_VALIDATION
#endif
#define DELAY_CONFIRMATION // Returns PurchaseProcessingResult.Pending from ProcessPurchase, then calls ConfirmPendingPurchase after a delay

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.UI;
#if RECEIPT_VALIDATION
using UnityEngine.Purchasing.Security;
#endif

/// <summary>
/// An example of basic Unity IAP functionality.
/// To use with your account, configure the product ids (AddProduct)
/// and Google Play key (SetPublicKey).
/// </summary>
public class IAPManager : MonoBehaviour, IStoreListener
{
    // Unity IAP objects 
    private IStoreController m_Controller;
    private IAppleExtensions m_AppleExtensions;

    #pragma warning disable 0414
    private bool m_IsGooglePlayStoreSelected = true;

    private string m_LastTransationID;
    private string m_LastReceipt;

    private bool m_PurchaseInProgress;

    #if RECEIPT_VALIDATION
    private CrossPlatformValidator validator;
    #endif

    /// <summary>
    /// This will be called when Unity IAP has finished initialising.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_Controller = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions> ();

        // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
        // On non-Apple platforms this will have no effect; OnDeferred will never be called.
        m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

        Debug.Log("Available items:");
        foreach (var item in controller.products.all)
        {
            if (item.availableToPurchase)
            {
                Debug.Log(string.Join(" - ",
                    new[]
                    {
                        item.metadata.localizedTitle,
                        item.metadata.localizedDescription,
                        item.metadata.isoCurrencyCode,
                        item.metadata.localizedPrice.ToString(),
                        item.metadata.localizedPriceString,
                        item.transactionID,
                        item.receipt
                    }));
            }
        }
    }

    /// <summary>
    /// This will be called when a purchase completes.
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
        Debug.Log("Receipt: " + e.purchasedProduct.receipt);

        m_LastTransationID = e.purchasedProduct.transactionID;
        m_LastReceipt = e.purchasedProduct.receipt;
        m_PurchaseInProgress = false;

        #if RECEIPT_VALIDATION
        // Local validation is available for GooglePlay and Apple stores
        if (m_IsGooglePlayStoreSelected || Application.platform == RuntimePlatform.IPhonePlayer) 
        {
            try 
            {
                var result = validator.Validate(e.purchasedProduct.receipt);
                Debug.Log("Receipt is valid. Contents:");
                foreach (IPurchaseReceipt productReceipt in result) 
                {
                    Debug.Log(productReceipt.productID);
                    Debug.Log(productReceipt.purchaseDate);
                    Debug.Log(productReceipt.transactionID);

                    GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
                    if (null != google) 
                    {
                        Debug.Log(google.purchaseState);
                        Debug.Log(google.purchaseToken);
                    }

                    AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
                    if (null != apple) 
                    {
                        Debug.Log(apple.originalTransactionIdentifier);
                        Debug.Log(apple.subscriptionExpirationDate);
                        Debug.Log(apple.cancellationDate);
                        Debug.Log(apple.quantity);
                    }
                }
            } 
            catch (IAPSecurityException) 
            {
                Debug.Log("Invalid receipt, not unlocking content");
                return PurchaseProcessingResult.Complete;
            }
        }
        #endif

        // You should unlock the content here.

        // Indicate if we have handled this purchase. 
        //   PurchaseProcessingResult.Complete: ProcessPurchase will not be called
        //     with this product again, until next purchase.
        //   PurchaseProcessingResult.Pending: ProcessPurchase will be called 
        //     again with this product at next app launch. Later, call 
        //     m_Controller.ConfirmPendingPurchase(Product) to complete handling
        //     this purchase. Use to transactionally save purchases to a cloud
        //     game service. 
        #if DELAY_CONFIRMATION
        StartCoroutine(ConfirmPendingPurchaseAfterDelay(e.purchasedProduct));
        return PurchaseProcessingResult.Pending;
        #else
        return PurchaseProcessingResult.Complete;
        #endif
    }

    #if DELAY_CONFIRMATION
    private HashSet<string> m_PendingProducts = new HashSet<string>();

    private IEnumerator ConfirmPendingPurchaseAfterDelay(Product p)
    {
        m_PendingProducts.Add(p.definition.id);
        Debug.Log("Delaying confirmation of " + p.definition.id + " for 5 seconds.");

        yield return new WaitForSeconds(5f);

        Debug.Log("Confirming purchase of " + p.definition.id);
        m_Controller.ConfirmPendingPurchase(p);
        m_PendingProducts.Remove(p.definition.id);
    }
    #endif

    /// <summary>
    /// This will be called is an attempted purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
        Debug.Log("Purchase failed: " + item.definition.id);
        Debug.Log(r);

        m_PurchaseInProgress = false;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Billing failed to initialize!");
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                Debug.Log("Billing disabled!");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                Debug.Log("No products available for purchase!");
                break;
        }
    }

    public void Awake()
    {
        var module = StandardPurchasingModule.Instance();

        var builder = ConfigurationBuilder.Instance(module);

        builder.Configure<IGooglePlayConfiguration>().SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA1zc86+vA0LpbZ8vfxaCmaCooVEh02IVEPN3yFw9X4rYB8phkf6XPhKNPO0N6GetJ5vrwF5+InKgizt6v12DtTXVX0ewnLakrfXEcXHE7fOHnMoDOhpPkdd0D3pvJHm0vQ+jf57UJfgh9VXPJE+d2zcLfZi4kR1oYP/5+tBV27yBcDYU/VSFdFhj06xv3olv+CBmLvOy7HBGP3Go0tV1SZdwTfxLUjhWp+c5Xoa9/ncrWVxdD8Qic+4HvKvYTu4DXX71EB6nhT03/8v5FeKsQfJK51hzX3Ylr+njMfImpK3siwzH+am6ChMXMjKeR1YlCumekUb+QbzC8+nuDQb4KkQIDAQAB");
        m_IsGooglePlayStoreSelected = Application.platform == RuntimePlatform.Android && module.androidStore == AndroidStore.GooglePlay;

        // Define our products.
        // In this case our products have the same identifier across all the App stores,
        // except on the Mac App store where product IDs cannot be reused across both Mac and
        // iOS stores.
        // So on the Mac App store our products have different identifiers,
        // and we tell Unity IAP this by using the IDs class.
        builder.AddProduct("subscription", ProductType.Subscription);
        

        #if RECEIPT_VALIDATION
        string appIdentifier;
        #if UNITY_5_6_OR_NEWER
        appIdentifier = Application.identifier;
        #else
        appIdentifier = Application.bundleIdentifier;
        #endif
        validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), appIdentifier);
        #endif

        // Now we're ready to initialize Unity IAP.
        UnityPurchasing.Initialize(this, builder);
    }

    public void Buy()
    {
        if (m_PurchaseInProgress == true) 
        {
            Debug.Log("Please wait, purchasing ...");
            return;
        }

        // Don't need to draw our UI whilst a purchase is in progress.
        // This is not a requirement for IAP Applications but makes the demo
        // scene tidier whilst the fake purchase dialog is showing.
        m_PurchaseInProgress = true;
        m_Controller.InitiatePurchase(m_Controller.products.WithID("subscription"), "aDemoDeveloperPayload"); 
    }

    /// <summary>
    /// This will be called after a call to IAppleExtensions.RestoreTransactions().
    /// </summary>
    private void OnTransactionsRestored(bool success)
    {
        Debug.Log("Transactions restored.");
    }

    /// <summary>
    /// iOS Specific.
    /// This is called as part of Apple's 'Ask to buy' functionality,
    /// when a purchase is requested by a minor and referred to a parent
    /// for approval.
    /// 
    /// When the purchase is approved or rejected, the normal purchase events
    /// will fire.
    /// </summary>
    /// <param name="item">Item.</param>
    private void OnDeferred(Product item)
    {
        Debug.Log("Purchase deferred: " + item.definition.id);
    }

    private void InitUI(IEnumerable<Product> items)
    {
        /*
        // Show Restore button on supported platforms
        if (! (Application.platform == RuntimePlatform.IPhonePlayer) )
        {
            GetRestoreButton().gameObject.SetActive(false);
        }

        // Initialize my button event handling
        GetBuyButton().onClick.AddListener(() => { 
            if (m_PurchaseInProgress == true) {
                Debug.Log("Please wait, purchasing ...");
                return;
            }

            // Don't need to draw our UI whilst a purchase is in progress.
            // This is not a requirement for IAP Applications but makes the demo
            // scene tidier whilst the fake purchase dialog is showing.
            m_PurchaseInProgress = true;
            m_Controller.InitiatePurchase(m_Controller.products.all[0], "aDemoDeveloperPayload"); 
        });

        if (GetRestoreButton() != null)
        {
            GetRestoreButton().onClick.AddListener(() => {
                m_AppleExtensions.RestoreTransactions(OnTransactionsRestored);
            });
        }
        */
    }

    private void LogProductDefinitions()
    {
        var products = m_Controller.products.all;
        foreach (var product in products) {
            #if UNITY_5_6_OR_NEWER
            Debug.Log(string.Format("id: {0}\nstore-specific id: {1}\ntype: {2}\nenabled: {3}\n", product.definition.id, product.definition.storeSpecificId, product.definition.type.ToString(), product.definition.enabled ? "enabled" : "disabled"));
            #else
            Debug.Log(string.Format("id: {0}\nstore-specific id: {1}\ntype: {2}\n", product.definition.id, product.definition.storeSpecificId, product.definition.type.ToString()));
            #endif
        }
    }
}
