using NSec.Cryptography;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CryptoTester : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text DebugLabel;
    
    void Start()
    {
        doThing();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void doThing()
    {
        var algo = SignatureAlgorithm.Ed25519;
        KeyCreationParameters keyParams = new KeyCreationParameters();
        keyParams.ExportPolicy = KeyExportPolicies.AllowPlaintextExport;
        using var key = Key.Create(algo, keyParams);

        var publicKey = key.PublicKey.Export(KeyBlobFormat.RawPublicKey);
        var privateKey = key.Export(KeyBlobFormat.RawPrivateKey);
        var data = Encoding.UTF8.GetBytes("Hello there");
        var signature = algo.Sign(key, data);

        //File.WriteAllBytes("public_key.txt", publicKey);
        //File.WriteAllBytes("private_key.txt", privateKey);
        //File.WriteAllBytes("data.txt", data);
        //File.WriteAllBytes("signature.txt", signature);

        if (algo.Verify(key.PublicKey, data, signature))
        {
            DebugLabel.text += "\nThis thing says it did its job right";
        }
        else
        {
            DebugLabel.text += "\nForgery!";
        }

    }
}
