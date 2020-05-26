
<?php
$email = $_POST["email"];

$conn = mysqli_connect('localhost', 'root','t1fkthsl', 'vity-game');
//check that connection happend
if(mysqli_connect_errno())
{
    echo "1: connection failed"; // error code #1 = connection failed
    exit();
}

$sql = "DELETE FROM playerTmp WHERE email ='".$email."'";
$result = $conn->query($sql);
$conn->close();
?>
