
<?php
$email = $_POST["email"];
$num = $_POST["number"];

require '/usr/share/php/libphp-phpmailer/class.phpmailer.php';
require '/usr/share/php/libphp-phpmailer/class.smtp.php';
$mail = new PHPMailer;
$mail->setFrom('admin@example.com');
$mail->addAddress($email);
$mail->Subject = '[VITY] Thank you for join us!';
$mail->Body = 'Your certification number is '.$num.'.';

$mail->isHTML(true);
$mail->AltBody = "This message is generated by plain text !";
$mail->IsSMTP();
$mail->SMTPSecure = 'ssl';
$mail->Host = 'ssl://smtp.gmail.com';
$mail->SMTPAuth = true;
$mail->Port = 465;
$mail->Username = 'wanted0301@gmail.com';
$mail->Password = '19734560';
if(!$mail->send()) {
  echo 'Email is not sent.';
  echo 'Email error: ' . $mail->ErrorInfo;
} else {
  echo '해당 메일로 인증메일을 전송하였습니다. 인증 후 진행해주세요.';
}
?>
