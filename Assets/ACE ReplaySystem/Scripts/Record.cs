using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReplayData;
using Replay;

namespace Recorder
{
    public class Record : MonoBehaviour
    {
        [Header("ReplayManager")]
        [Tooltip("Drag and drop the Replay Manager of the scene here, not necessary if you put below the ReplayManager name.")]
        public ReplayManager replay;

        [Header("Replay Manager name in the scene")]
        [Tooltip("Insted of drag and dropping you can place the name of the ReplayManager here.")]
        //Replaymanager name in Scene
        [SerializeField] string replayManagerName = "ReplayManager";

        //Scripts of the GO that dont have to be disabled during replay
        [Header("Scripts to NOT disable")]
        [Tooltip("Drag and drop the scripts that dont have to be disabled during replay.")]
        [SerializeField] MonoBehaviour[] scripts = null;

        //List of recorded Frames 
        private List<Frame> frames = new List<Frame>();

        //RB recording
        private Rigidbody rigidBody;

        //animator recording
        private Animator animator;
        private bool startedRecording = false;
        private int animFramesRecorded = 0;

        //AudioSource recording
        private AudioSource audioSource;
        private bool audioPlay = false;
        private bool audioStarted = false;

        //Particle system recording
        private ParticleSystem particle;

        //Useful to know if it was instantiated during the recording
        private int numberFirstFrame;
        private bool instantiated = false;

        //Record Deleted while recording
        // if not deleted it will remain -1, if deleted it will take the frame where it was deleted
        private int recordDeletedFrame = -1;
        //deleted go 
        private GameObject deletedGO;

        void Start()
        {
            //make sure replay is not NULL
            if (replay == null)
                replay = GameObject.Find(replayManagerName).GetComponent<ReplayManager>();

            //Get components
            rigidBody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            particle = GetComponent<ParticleSystem>();

            if (replay != null)
            {
                replay.AddRecord(this);

                //first frame initialization, useful to know the frame where an instantiated go was spawned
                numberFirstFrame = replay.GetReplayLength();
                //look if it is an instantiated go
                if (numberFirstFrame != 0) instantiated = true;
            }
            else
                Debug.LogWarning("ReplayManager not found, make sure there is a replayManger in the scene. Make sure to assign it by drag and drop or by puting the correct replayManagerName");
        }

        public void RecordFrame()
        {
            //record transforms
            Frame frame = new Frame(transform.position, transform.rotation, transform.localScale);

            //record animations
            RecordAnimation();

            //record rigidBody velocities
            RecordRigidBody(frame);

            //record audio data
            RecordAudio(frame);

            //record particle data
            RecordParticle(frame);

            //Add new frame to the list
            AddFrame(frame);
        }

        //Add frame, if list has maxLength remove first element
        void AddFrame(Frame frame)
        {
            if (GetLength() >= replay.GetMaxLength())
            {
                frames.RemoveAt(0);
            }

            frames.Add(frame);
        }

        //Record RB velocities
        void RecordRigidBody(Frame frame)
        {
            if (rigidBody != null)
                frame.SetRBVelocities(rigidBody.linearVelocity, rigidBody.angularVelocity);
        }

        //Record Animation
        void RecordAnimation()
        {
            if (animator != null && startedRecording == false)
            {
                animator.StartRecording(replay.GetAnimatorRecordLength());
                startedRecording = true;
            }
        }

        //Record Audio
        void RecordAudio(Frame frame)
        {
            if (audioSource != null)
            {
                if (audioSource.isPlaying && audioStarted == false)
                {
                    audioStarted = true;
                    audioPlay = true;
                }
                else if (audioStarted && audioPlay)
                {
                    audioPlay = false;
                }
                else if (audioSource.isPlaying == false && audioStarted)
                {
                    audioStarted = false;
                }

                frame.SetAudioData(new AudioData(audioPlay, audioSource.pitch, audioSource.volume, audioSource.panStereo, audioSource.spatialBlend, audioSource.reverbZoneMix));
            }
        }

        //Record Particle
        void RecordParticle(Frame frame)
        {
            if (particle != null)
            {
                if (particle.isEmitting)
                    frame.SetParticleData(particle.time);
                else
                    frame.SetParticleData(0f);
            }
        }

        //Prepare to record again
        public void ClearFrameList()
        {
            frames.Clear();
            animFramesRecorded = 0;
            numberFirstFrame = 0;
            instantiated = false;
            deletedGO = null;
            recordDeletedFrame = -1;
        }

        //used for the animator recording
        public void SetStartRecording(bool b)
        {
            startedRecording = b;
        }

        //when enter replay mode set to TRUE and when exit set to FALSE
        public void SetKinematic(bool b)
        {
            if (rigidBody != null)
                rigidBody.isKinematic = b;
        }

        //rearrange instantiation and deletion frames
        public void UpdateFramesNum()
        {
            if (replay.GetReplayLength() == replay.GetMaxLength())
            {
                //instantiated record
                if (numberFirstFrame > 0)
                    numberFirstFrame--;

                //deleted record
                if (recordDeletedFrame != -1 && recordDeletedFrame > 0)
                {
                    recordDeletedFrame--;

                    //delete frames that are out of the replay
                    if (instantiated == false || (instantiated == true && numberFirstFrame == 0))
                        if (frames.Count > 0)
                            frames.RemoveAt(0);
                }
            }
        }

        public void ManageScripts(bool enable)
        {
            MonoBehaviour[] allScripts = gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in allScripts)
            {
                if (script != this && CheckScriptsList(script) == false)
                    script.enabled = enable;
            }
        }

        bool CheckScriptsList(MonoBehaviour s)
        {
            bool ret = false;

            foreach (MonoBehaviour script in scripts)
            {
                if (script == s)
                    ret = true; break;
            }

            return ret;
        }

        //SETTERS
        public void SetDeletedGameObject(GameObject go) { deletedGO = go; }
        public void SetRecordDeletedFrame(int frame) { recordDeletedFrame = frame; }

        public void IncreaseRecordedAnimatorFrames() { animFramesRecorded++; }

        // GETTERS
        public int GetLength() { return frames.Count; }
        public Frame GetFrameAtIndex(int index)
        {
            if (index < 0) return null;

            return index >= frames.Count ? null : frames[index];
        }

        //instantiation and deletion
        public int GetFirstFrameIndex() { return numberFirstFrame; }
        public bool IsInstantiated() { return instantiated; }
        public int GetRecordDeletedFrame() { return recordDeletedFrame; }

        //records Go and deleted GO
        public GameObject GetGameObject() { return gameObject; }
        public GameObject GetDeletedGO() { return deletedGO; }

        // other recorded components
        public Rigidbody GetRigidbody() { return rigidBody; }
        public Animator GetAnimator() { return animator; }
        public int GetAnimFramesRecorded() { return animFramesRecorded; }
        public AudioSource GetAudioSource() { return audioSource; }
        public ParticleSystem GetParticle() { return particle; }
    }
}