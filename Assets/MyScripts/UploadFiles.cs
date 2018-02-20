using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class UploadFiles : MonoBehaviour {

    private void Start()
    {
        // File located on disk
        string local_file = @"D:\UnityProjects\Platformer\Pyre2D\Checkpoint0.png";

    // Get a reference to the storage service, using the default Firebase App
    FirebaseStorage storage = FirebaseStorage.DefaultInstance;

    // Create a storage reference from our storage service
     StorageReference storage_ref = storage.GetReferenceFromUrl("gs://compscreenshottest.appspot.com/");

    // Create a reference to the file you want to upload
     StorageReference screenshot_ref = storage_ref.Child("screenshots/checkpoint0.png");

        Debug.Log("Start ran. Refreneces made");
        // Upload the file to the path "images/rivers.jpg"
        screenshot_ref.PutFileAsync(local_file).ContinueWith((Task<StorageMetadata> task) => {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
                // Uh-oh, an error occurred!
            }
            else
            {
                // Metadata contains file metadata such as size, content-type, and download URL.
                StorageMetadata metadata = task.Result;
                string download_url = metadata.DownloadUrl.ToString();
                Debug.Log("Finished uploading...");
                Debug.Log("download url = " + download_url);
            }
        });
    }



}
