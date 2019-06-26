package [패키지명]];

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

///BootReceiver 등록 서비스 실행 예제.

public class BootReceiver extends BroadcastReceiver {

    public void onReceive(Context context, final Intent intent) {

        if (intent != null && intent.getAction() != null && intent.getAction().equals(Intent.ACTION_BOOT_COMPLETED)) {
            Intent i = new Intent(context, [클래스명]].class);
            context.startService(i);
            Log.d("BootReceiver", "Run the service through ACTION_BOOT_COMPLETED.");
            return;
        }
    }

}