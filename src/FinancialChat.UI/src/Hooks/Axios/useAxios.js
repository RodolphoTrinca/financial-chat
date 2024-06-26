import { useState, useEffect } from "react";

export const useAxios = (configObject, transformResponse) => {
    const {
        axiosInstance,
        method,
        url,
        requestConfig = {}
    } = configObject;

    const [response, setResponse] = useState(null);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(true);
    const [reload, setReload] = useState(0);

    const refetch = () => {
        setReload(prev => prev + 1);
    }

    useEffect(() => {
        const controller = new AbortController();
        setTimeout(() => controller.abort(), 1000);

        const fetchData = async () => {
            try {
                setLoading(true);
                const res = await axiosInstance[method.toLowerCase()](url, {
                    ...requestConfig,
                    signal: controller.signal,
                });

                if(res.status > 199 && res.status < 300)
                {
                    const data = transformResponse != undefined ? transformResponse(res.data) : res.data;
                    setResponse(data);
                    return;
                }

                setError(res.data);
            } catch (err) {
                console.error(err, err.stack);
                setError(err.message);
            }finally{
                setLoading(false);
            }
        }

        fetchData();

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [reload]);

    return [response, error, loading, refetch];
}