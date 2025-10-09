// using System;
// using GoogleMobileAds.Ump.Api;
// using UnityEngine;
//
// public class GoogleMobileAdsConsentController : MonoBehaviour
// {
//     public bool CanRequestAds => ConsentInformation.CanRequestAds();
//
//     public void GatherConsent(Action<string> onComplete)
//     {
//         var requestParams = new ConsentRequestParameters
//         {
//             ConsentDebugSettings = new ConsentDebugSettings
//             {
//                 // DebugGeography = DebugGeography.EEA,
//             }
//         };
//
//         ConsentInformation.Update(requestParams, (FormError updateError) =>
//         {
//             if (updateError != null)
//             {
//                 onComplete(updateError.Message);
//                 return;
//             }
//
//             ConsentForm.LoadAndShowConsentFormIfRequired((FormError showError) =>
//             {
//                 onComplete?.Invoke(showError?.Message);
//             });
//         });
//     }
// }