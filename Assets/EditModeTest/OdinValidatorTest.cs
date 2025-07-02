using NUnit.Framework;
using Sirenix.OdinValidator.Editor;
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

public class OdinValidatorTest
{
    [Test]
    public void ValidatorTest()
    {
        var profile = ValidationProfile.MainValidationProfile;
        var errorStr = string.Empty;
        
        Debug.Log($"Profile: {profile.name}");
        using (var session = new ValidationSession(profile))
        {
            foreach (var result in session.ValidateEverythingEnumerator(openClosedScenes: true, showProgressBar: false))
            {
                if (result.ResultType != ValidationResultType.Error)
                {
                    continue;
                }

                errorStr += $"[{result.ValidatorType}] {result.Message}\n";
            }
        }
        Assert.IsEmpty(errorStr, $"Odin Validation Failed.\nErrors: {errorStr}");
    }
}
