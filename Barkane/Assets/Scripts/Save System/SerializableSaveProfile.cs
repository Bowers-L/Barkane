using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class SerializableSaveProfile
{
    public string profileName;
    public bool completionStatus;
    public float playTimeInSeconds;
    public System.DateTime lastSaved;
    public System.DateTime lastImproved;
    public string lastLevel;
    public string cosmetic;
    public int lastLevelWorldNum;
    public int lastLevelNum;

    public string[] cosmetics_Keys;
    public bool[]   cosmetics_Values;

    public string[] levelFolds_Keys;
    public int[] levelFolds_Values;

    public string[] unlocks_Keys;
    public bool[]   unlocks_Values;

    public static SerializableSaveProfile FromSaveProfile(SaveProfile saveProfile)
    {
        if (saveProfile == null) return null;

        SerializableSaveProfile ssp = new SerializableSaveProfile();

        ssp.profileName = saveProfile.GetProfileName();
        ssp.completionStatus = saveProfile.GetCompletionStatus();
        ssp.playTimeInSeconds = saveProfile.GetPlayTimeInSeconds();
        ssp.lastSaved = saveProfile.GetLastSaved();
        ssp.lastLevel = saveProfile.GetLastLevelString();
        ssp.cosmetic = saveProfile.GetCosmetic();
        ssp.lastLevelNum = saveProfile.GetLastLevelNum();
        ssp.lastLevelWorldNum = saveProfile.GetLastLevelWorldNum();
        ssp.lastImproved = saveProfile.GetLastImproved();

        ssp.cosmetics_Keys = saveProfile.GetCosmeticsDictionary().Keys.ToArray();
        ssp.levelFolds_Keys = saveProfile.GetFoldsDictionary().Keys.ToArray();
        ssp.unlocks_Keys = saveProfile.GetLevelUnlocksDictionary().Keys.ToArray();

        ssp.cosmetics_Values = saveProfile.GetCosmeticsDictionary().Values.ToArray();
        ssp.levelFolds_Values = saveProfile.GetFoldsDictionary().Values.ToArray();
        ssp.unlocks_Values = saveProfile.GetLevelUnlocksDictionary().Values.ToArray();


        return ssp;
    }

    public SaveProfile ToSaveProfile()
    {
        SaveProfile sp = new SaveProfile(profileName);
        
        sp.SetCompletionStatus(completionStatus);
        sp.SetPlayTimeInSeconds(playTimeInSeconds);
        sp.SetLastSaved(lastSaved);
        sp.SetLastLevelString(lastLevel);
        sp.SetLastLevelNum(lastLevelNum);
        sp.SetLastLevelWorldNum(lastLevelWorldNum);
        sp.SetCosmetic(cosmetic);
        sp.SetLastImproved(lastImproved);


        Dictionary<string, bool> bools = new Dictionary<string, bool>(cosmetics_Keys.Length);
        for (int i = 0; i < cosmetics_Keys.Length; i++)
            bools.Add(cosmetics_Keys[i], cosmetics_Values[i]);
        sp.SetCosmeticsDictionary(bools);

        if (levelFolds_Keys != null)
        {
            Dictionary<string, int> ints = new Dictionary<string, int>(levelFolds_Keys.Length);
            for (int i = 0; i < levelFolds_Keys.Length; i++)
                ints.Add(levelFolds_Keys[i], levelFolds_Values[i]);
            sp.SetFoldsDictionary(ints);
        } else
        {
            Debug.LogWarning("[SerializableSaveProfile] The saved Levels dictionary had no keys. No levels have been played.");
        }

        if (unlocks_Keys != null)
        {
            Dictionary<string, bool> bools2 = new Dictionary<string, bool>(unlocks_Keys.Length);
            for (int i = 0; i < unlocks_Keys.Length; i++)
                bools2.Add(unlocks_Keys[i], unlocks_Values[i]);
            sp.SetLevelUnlocksDictionary(bools2);
        } else
        {
            Debug.LogWarning("[SerializableSaveProfile] The Level Unlocks dictionary had no keys. No levels have been played.");
        }
        return sp;
    }
}
