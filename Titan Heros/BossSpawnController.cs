using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnController : MonoBehaviour
{
    private Animator anim;

    public GameObject bossSpawnCamera;

    public EnemySpawner enemySpawner;

    public float delayBefore_SpawningBoss = 2f;
    public float delayBefore_BossFightStarts = 4f;
    public float shakeTime = 0.5f;

    private ShakeCamera shakeCamera;

    void Awake()
    {
        anim = GetComponent<Animator>();
        shakeCamera = GetComponent<ShakeCamera>();
    }

    public void StartBossSpawn()
    {
        StartCoroutine(BossSpawnWithDelay());
    }

    private IEnumerator BossSpawnWithDelay()
    {
        // WaitForSeconds�� Time.timeScale�� �ݿ��� �ð��� ��ٸ��� WaitForSecondsRealtime�� ���� �ð��� ��ٸ���
        yield return new WaitForSecondsRealtime(delayBefore_SpawningBoss);

        // Time.timeScale = 0f���� �Ͽ� ������ �Ͻ������� ���ȿ��� �ִϸ��̼��� �����ϰ� ���� ������
        // Boss Spawn Controller�� ��� ������������ Animator������Ʈ�� Update Mode�� Unscaled Time���� �Ͽ�
        // Animator�� Time.timeScale�� ���������� ������Ʈ�ǵ��� �ؾ���
        Time.timeScale = 0f;

        bossSpawnCamera.SetActive(true);

        anim.Play(AnimationTags.SLIDE_IN_ANIMATION);
    }

    private void ShakeAndSpawn()
    {
        StartCoroutine(ShakeTheCameraAndSpawnTheBoss());
    }

    private IEnumerator ShakeTheCameraAndSpawnTheBoss()
    {
        shakeCamera.InitializeValues(shakeTime);

        enemySpawner.SpawnBoss(0);

        yield return new WaitForSecondsRealtime(shakeTime + delayBefore_BossFightStarts);

        // Animator������Ʈ�� Update Mode�� Unscaled Time���� �ؾ���
        Time.timeScale = 1f;

        bossSpawnCamera.SetActive(false);
    }
}
