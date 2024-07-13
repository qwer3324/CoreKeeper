using UnityEngine;
using static SoundManager;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [Header("BGM")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("Ambience")]
    public AudioClip[] ambienceClips;
    public float ambienceVolume;
    AudioSource ambiencePlayer;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Bgm { Nature, Sea, MoldDungeon, BossSlime, Shaman }
    public enum Sfx
    {
        InventoryOpen, InventoryClose, ItemSwitchDrag, ItemSwitchDrop, MenuSelect, MenuDeny, Eat, Drink, PickUp, Punch,
        PlayerHit = 11, MagicMirror, CrabAttack, CrabDie, CrabHit, BossSlimeAttack, BossSlimeBerserk, BossSlimeDie, BowHit, BulletHit,
        CupidBow, CupidBowHit, Fireball, FireballImpact = 25, FireWhoosh, Galaxite = 28, GalaxiteHit, GameOver, Glop, GolemAttack, GolemDie,
        LarvaHit, LarvaDie = 37, Musket = 39, SlimeAttack, SlimeDie, SlimeHit, SlimeJump, SlimeProjectile = 46, SlimeStaff, MoldTentacleAttack,
        Slingshot, SlingshotHit, UIPlace, WoodBow, WoodSword, Hit, UIMenuSelect, GrassFootStep, SeaFootStep, MoldDungeonFootStep, MoldTentacleDie, GolemHit, GolemWakeUp
    }

    private new void Awake()
    {
        base.Awake();
        Init();
    }
    private void Start()
    {
        PlayBgm(Bgm.Nature, true);
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClips[0];

        GameObject ambObject = new GameObject("AmbiencePlayer");
        ambObject.transform.parent = transform;
        ambiencePlayer = ambObject.AddComponent<AudioSource>();
        ambiencePlayer.playOnAwake = false;
        ambiencePlayer.loop = true;
        ambiencePlayer.volume = ambienceVolume;
        ambiencePlayer.clip = ambienceClips[0];

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for(int i = 0; i < channels; i++) 
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlayBgm(Bgm bgm, bool isPlay = true)
    {
        if( bgmPlayer.clip == bgmClips[(int)bgm])
        {
            if (isPlay)
                bgmPlayer.Play();
            else
                bgmPlayer.Stop();

            return;
        }

        bgmPlayer.clip = bgmClips[(int)bgm];

        if (isPlay)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int i = 0;i  < channels;i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying) 
            {
                continue;
            }

            int randIndex = 0;
            if(sfx == Sfx.Punch)
            {
                randIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + randIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
